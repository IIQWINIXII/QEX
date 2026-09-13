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

export function start(x, y) {
    isDrawing = true;
    startX = x;
    startY = y;
    savedData = ctx.getImageData(0, 0, ctx.canvas.width, ctx.canvas.height);
    ctx.beginPath();
    ctx.moveTo(x, y);
}

export function move(x, y) {
    if (!isDrawing) return;
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