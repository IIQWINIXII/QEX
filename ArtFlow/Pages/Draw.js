// Pages/Draw.js
let ctx = null;
let canvasEl = null;
let scrollEl = null;

let isDrawing = false;
let startX = 0, startY = 0;
let savedData = null;
let tool = 'pencil';
let color = '#000000';
let size = 3;
let background = '#FFFFFF';

// --- Зум ---
let zoom = 1.0;
const ZOOM_MIN = 0.1;
const ZOOM_MAX = 8.0;
const ZOOM_STEP = 0.1;

/* ============================================================
 *  Инициализация
 * ============================================================ */

export function init(canvas, bg) {
    if (!canvas) return;
    canvasEl = canvas;
    ctx = canvas.getContext('2d');
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
    if (typeof bg === 'string') background = bg;

    fillBackground();
    applyZoom(); // на случай, если зум уже не 1
}

/**
 * Пересоздаёт буфер холста под новый размер. Сбрасывает зум к 100%.
 */
export function resize(w, h, bg) {
    if (!ctx) return;
    ctx.canvas.width = w;
    ctx.canvas.height = h;
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
    if (typeof bg === 'string') background = bg;

    zoom = 1.0;
    applyZoom();
    fillBackground();
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
 *  Зум колесом (без перерисовки Blazor)
 * ============================================================ */

let wheelHandler = null;

export function hookWheel(scrollContainer, canvas) {
    if (!scrollContainer || !canvas) return;
    scrollEl = scrollContainer;
    canvasEl = canvas;

    if (wheelHandler) return;

    wheelHandler = function (e) {
        e.preventDefault(); // отключаем скролл

        const rect = canvasEl.getBoundingClientRect();
        // позиция курсора ОТНОСИТЕЛЬНО канваса (в его текущих экранных px)
        const cx = e.clientX - rect.left;
        const cy = e.clientY - rect.top;

        // "якорь" в координатах буфера, который должен остаться под курсором
        const anchorX = cx / zoom;
        const anchorY = cy / zoom;

        const dir = e.deltaY < 0 ? 1 : -1;
        const newZoom = clamp(zoom + dir * ZOOM_STEP, ZOOM_MIN, ZOOM_MAX);
        if (newZoom === zoom) return;

        zoom = newZoom;

        // применяем размеры и сдвигаем скролл, чтобы курсор остался на месте
        applyZoomAndKeepAnchor(anchorX, anchorY, e.clientX, e.clientY);
    };

    scrollEl.addEventListener('wheel', wheelHandler, { passive: false });
}

export function unhookWheel() {
    if (scrollEl && wheelHandler) {
        scrollEl.removeEventListener('wheel', wheelHandler, { passive: false });
    }
    scrollEl = null;
    wheelHandler = null;
}

function applyZoom() {
    if (!canvasEl) return;
    canvasEl.style.width = (canvasEl.width * zoom) + 'px';
    canvasEl.style.height = (canvasEl.height * zoom) + 'px';
}

/**
 * Применяет зум и корректирует scrollLeft/scrollTop так,
 * чтобы точка (anchorX, anchorY) в буфере осталась под курсором.
 */
function applyZoomAndKeepAnchor(anchorX, anchorY, clientX, clientY) {
    if (!canvasEl || !scrollEl) return;

    // Координаты курсора относительно контейнера скролла (включая padding)
    const scrollRect = scrollEl.getBoundingClientRect();
    const cursorInScrollX = clientX - scrollRect.left + scrollEl.scrollLeft;
    const cursorInScrollY = clientY - scrollRect.top + scrollEl.scrollTop;

    // Запоминаем положение курсора в контейнере до зума
    const beforeX = clientX - scrollRect.left;
    const beforeY = clientY - scrollRect.top;

    // Применяем новый размер
    applyZoom();

    // После зума новая позиция "якоря" относительно начала канваса (в экранных px)
    const canvasRect = canvasEl.getBoundingClientRect();
    const newCanvasInScrollX = canvasRect.left - scrollRect.left + scrollEl.scrollLeft;
    const newCanvasInScrollY = canvasRect.top - scrollRect.top + scrollEl.scrollTop;

    // Куда должен попасть якорь в новой системе
    const targetX = newCanvasInScrollX + anchorX * zoom;
    const targetY = newCanvasInScrollY + anchorY * zoom;

    // Сдвигаем скролл так, чтобы точка осталась под курсором
    const dx = targetX - cursorInScrollX;
    const dy = targetY - cursorInScrollY;

    scrollEl.scrollLeft += dx;
    scrollEl.scrollTop += dy;

    // Небольшая подстройка, если у контейнера есть padding
    // (обычно не требуется, но оставим комментарий на будущее)
    void beforeX; void beforeY;
}

function clamp(v, a, b) { return v < a ? a : v > b ? b : v; }

/* ============================================================
 *  Рисование
 * ============================================================ */

function toCanvas(clientX, clientY) {
    const rect = ctx.canvas.getBoundingClientRect();
    const scaleX = ctx.canvas.width / rect.width;
    const scaleY = ctx.canvas.height / rect.height;
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

/**
 * Очистка холста. Если фон задан — перекрашивает его, иначе полностью прозрачный.
 */
export function clear() {
    if (!ctx) return;
    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    if (background && background !== 'transparent') {
        ctx.fillStyle = background;
        ctx.fillRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    }
}

/**
 * Возвращает PNG data URL текущего холста — используется для сохранения файла.
 */
export function toDataUrl() {
    if (!ctx) return '';
    return ctx.canvas.toDataURL('image/png');
}

export function dispose() {
    ctx = null;
    canvasEl = null;
    scrollEl = null;
    isDrawing = false;
    savedData = null;
    zoom = 1.0;
    wheelHandler = null;
}