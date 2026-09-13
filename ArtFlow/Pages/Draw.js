// Pages/Draw.js
let ctx = null;
let isDrawing = false;
let startX = 0, startY = 0;
let savedData = null;
let tool = 'pencil';
let color = '#000000';
let size = 3;
let background = '#FFFFFF';

/**
 * Инициализация модуля.
 * @param {HTMLCanvasElement} canvas
 * @param {string} [bg] — цвет фона ('#FFFFFF', '#000000' или 'transparent')
 */
export function init(canvas, bg) {
    if (!canvas) return;
    ctx = canvas.getContext('2d');
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
    if (typeof bg === 'string') background = bg;
    fillBackground();
}

/**
 * Пересоздаёт буфер холста под новый размер (содержимое сбрасывается).
 * Вызывается из Blazor при применении настроек холста.
 */
export function resize(w, h, bg) {
    if (!ctx) return;
    ctx.canvas.width = w;
    ctx.canvas.height = h;
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
    if (typeof bg === 'string') background = bg;
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

// Преобразование координат окна -> координаты буфера канваса
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
    isDrawing = false;
    savedData = null;
}