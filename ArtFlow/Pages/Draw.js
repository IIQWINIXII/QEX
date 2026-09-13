// Pages/Draw.js
let ctx = null;
let canvasEl = null;
let stageEl = null;

let isDrawing = false;
let startX = 0, startY = 0;
let savedData = null;
let tool = 'pencil';
let color = '#000000';
let size = 3;
let background = '#FFFFFF';

// --- Зум и панорама (transform: translate + scale) ---
let zoom = 1.0;
let panX = 0;   // смещение канваса в пикселях stage
let panY = 0;
const ZOOM_MIN = 0.05;
const ZOOM_MAX = 16.0;
const ZOOM_FACTOR = 1.12;   // множитель на один "щелчок" колеса

/* ============================================================
 *  Инициализация
 * ============================================================ */

export function init(canvas, stage, bg) {
    if (!canvas || !stage) return;
    canvasEl = canvas;
    stageEl = stage;
    ctx = canvas.getContext('2d');
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
    if (typeof bg === 'string') background = bg;

    fillBackground();

    // Центрируем канвас в stage при старте
    centerCanvas();
}

export function resize(w, h, bg) {
    if (!ctx) return;
    ctx.canvas.width = w;
    ctx.canvas.height = h;
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
    if (typeof bg === 'string') background = bg;

    zoom = 1.0;
    centerCanvas();
    fillBackground();
}

function centerCanvas() {
    if (!canvasEl || !stageEl) return;
    const sw = stageEl.clientWidth;
    const sh = stageEl.clientHeight;
    const cw = canvasEl.width;
    const ch = canvasEl.height;
    panX = Math.round((sw - cw) / 2);
    panY = Math.round((sh - ch) / 2);
    applyTransform();
}

function applyTransform() {
    if (!canvasEl) return;
    // transform-origin: 0 0 — обязательно (задан в CSS)
    canvasEl.style.transform =
        `translate(${panX}px, ${panY}px) scale(${zoom})`;
    // CSS-размер оставляем равным буферу — transform сам всё масштабирует
    canvasEl.style.width = canvasEl.width + 'px';
    canvasEl.style.height = canvasEl.height + 'px';
}

function fillBackground() {
    if (!ctx) return;
    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    if (background && background !== 'transparent') {
        ctx.fillStyle = background;
        ctx.fillRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    }
}

export function sync(t, c, s) {
    tool = t;
    color = c;
    size = s;
}

/* ============================================================
 *  Зум колесом — относительно курсора
 * ============================================================ */

let wheelHandler = null;

export function hookWheel(stage) {
    if (!stage) return;
    stageEl = stage;

    if (wheelHandler) return;

    wheelHandler = function (e) {
        e.preventDefault();

        // Позиция курсора ОТНОСИТЕЛЬНО stage
        const stageRect = stageEl.getBoundingClientRect();
        const mx = e.clientX - stageRect.left;
        const my = e.clientY - stageRect.top;

        // Точка в системе координат канваса (в буферных px) под курсором
        // screen = pan + buf * zoom  →  buf = (mouse - pan) / zoom
        const bufX = (mx - panX) / zoom;
        const bufY = (my - panY) / zoom;

        // Меняем зум
        const factor = e.deltaY < 0 ? ZOOM_FACTOR : 1 / ZOOM_FACTOR;
        const newZoom = clamp(zoom * factor, ZOOM_MIN, ZOOM_MAX);
        if (Math.abs(newZoom - zoom) < 1e-6) return;
        zoom = newZoom;

        // Так двигаем pan, чтобы точка bufX/bufY оказалась под курсором:
        // mouse = pan + buf * zoom  →  pan = mouse - buf * zoom
        panX = mx - bufX * zoom;
        panY = my - bufY * zoom;

        applyTransform();
    };

    stageEl.addEventListener('wheel', wheelHandler, { passive: false });
}

export function unhookWheel() {
    if (stageEl && wheelHandler) {
        stageEl.removeEventListener('wheel', wheelHandler, { passive: false });
    }
    wheelHandler = null;
}

function clamp(v, a, b) { return v < a ? a : v > b ? b : v; }

/* ============================================================
 *  Рисование — используем getBoundingClientRect канваса,
 *  т.к. он теперь имеет transform: translate+scale
 * ============================================================ */

function toCanvas(clientX, clientY) {
    const rect = canvasEl.getBoundingClientRect();
    const scaleX = canvasEl.width / rect.width;
    const scaleY = canvasEl.height / rect.height;
    return {
        x: (clientX - rect.left) * scaleX,
        y: (clientY - rect.top) * scaleY
    };
}

export function start(clientX, clientY) {
    if (!ctx) return;
    const { x, y } = toCanvas(clientX, clientY);
    isDrawing = true;
    startX = x;
    startY = y;
    savedData = ctx.getImageData(0, 0, ctx.canvas.width, ctx.canvas.height);
    ctx.beginPath();
    ctx.moveTo(x, y);
}

export function move(clientX, clientY) {
    if (!isDrawing || !ctx) return;
    const { x, y } = toCanvas(clientX, clientY);
    ctx.putImageData(savedData, 0, 0);
    ctx.strokeStyle = color;
    ctx.lineWidth = size;

    if (tool === 'pencil') {
        ctx.lineTo(x, y);
        ctx.stroke();
    } else if (tool === 'line') {
        ctx.beginPath();
        ctx.moveTo(startX, startY);
        ctx.lineTo(x, y);
        ctx.stroke();
    } else if (tool === 'rect') {
        ctx.strokeRect(startX, startY, x - startX, y - startY);
    } else if (tool === 'circle') {
        const r = Math.max(Math.abs(x - startX), Math.abs(y - startY));
        ctx.beginPath();
        ctx.arc(startX, startY, r, 0, 2 * Math.PI);
        ctx.stroke();
    }
}

export function end() {
    isDrawing = false;
    savedData = null;
}

export function clear() {
    if (!ctx) return;
    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    if (background && background !== 'transparent') {
        ctx.fillStyle = background;
        ctx.fillRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    }
}

export function toDataUrl() {
    if (!ctx) return '';
    return ctx.canvas.toDataURL('image/png');
}

export function dispose() {
    ctx = null;
    canvasEl = null;
    stageEl = null;
    isDrawing = false;
    savedData = null;
    zoom = 1.0;
    panX = 0;
    panY = 0;
    wheelHandler = null;
}