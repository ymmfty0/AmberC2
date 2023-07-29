window.openWindow = function (url, title, width, height) {
    const left = window.screen.width / 2 - width / 2;
    const top = window.screen.height / 2 - height / 2;
    window.open(url, title, `width=${width},height=${height},left=${left},top=${top}`);
};