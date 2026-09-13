// Pages/Draw.js
let ctx = null;
let isDrawing = false;
let startX = 0, startY = 0;
let savedData = null;
let tool = 'pencil';
let color = '#000000';
let size = 3;

export function init(canvas) {
    ctx = canvas.getContext('2d');
    ctx.lineCap = 'round';
    ctx.lineJoin = 'round';
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
    const { x, y } = toCanvas(clientX, clientY);
    isDrawing = true;
    startX = x;
    startY = y;
    savedData = ctx.getImageData(0, 0, ctx.canvas.width, ctx.canvas.height);
    ctx.beginPath();
    ctx.moveTo(x, y);
}

export function move(clientX, clientY) {
    if (!isDrawing) return;
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
    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
}
export function dispose() {
    ctx = null;
    isDrawing = false;
    savedData = null;
}