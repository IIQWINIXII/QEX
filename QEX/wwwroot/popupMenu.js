window.popupMenu = {
    getAdjustedPosition: function (x, y, popupElement) {
        try {
            if (!popupElement) return [x, y];

            const rect = popupElement.getBoundingClientRect();
            const vw = Math.max(document.documentElement.clientWidth || 0, window.innerWidth || 0);
            const vh = Math.max(document.documentElement.clientHeight || 0, window.innerHeight || 0);

            let left = x;
            let top = y;

            // If popup would overflow right edge, move it left
            if (left + rect.width > vw) {
                left = Math.max(0, vw - rect.width - 8); // 8px margin
            }

            // If popup would overflow bottom edge, move it up
            if (top + rect.height > vh) {
                top = Math.max(0, vh - rect.height - 8);
            }

            // If left is negative, clamp to 0
            if (left < 0) left = 0;
            if (top < 0) top = 0;

            return [left, top];
        }
        catch (e) {
            return [x, y];
        }
    }
};
