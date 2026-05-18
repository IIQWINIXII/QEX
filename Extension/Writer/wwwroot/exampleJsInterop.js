// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}

window.writerInterop = {
    pickFile: async () => {
        return new Promise((resolve) => {
            const input = document.createElement('input');
            input.type = 'file';
            input.onchange = async (e) => {
                const file = e.target.files[0];
                if (!file) { resolve(null); return; }
                const arrayBuffer = await file.arrayBuffer();
                const bytes = Array.from(new Uint8Array(arrayBuffer));
                resolve({ fileName: file.name, content: bytes, contentType: file.type });
            };
            input.click();
        });
    },

    saveFile: (fileName, bytes) => {
        const blob = new Blob([new Uint8Array(bytes)], { type: 'application/octet-stream' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        a.remove();
        URL.revokeObjectURL(url);
    },

    promptFileName: (defaultName) => {
        return prompt("Save as", defaultName);
    }
};