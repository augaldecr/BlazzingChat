export const setTheme = (themeName) => {

    let newLink = document.createElement("link");
    newLink.setAttribute("id", "theme");
    newLink.setAttribute("rel", "stylesheet");
    newLink.setAttribute("type", "text/css");
    newLink.setAttribute("href", `css/app-${themeName}.css`);

    let head = document.getElementsByTagName("head")[0];
    head.querySelector("#theme").remove();

    head.appendChild(newLink);
}

export const downloadFile = (mimeType, base64String, fileName) =>
{
    var fileDataUrl = `data:${mimeType};base64,${base64String}`;
    fetch(fileDataUrl)
        .then(response => response.blob())
        .then(blob => {
            //create a link
            var link = window.document.createElement("a");
            link.href = window.URL.createObjectURL(blob, { type: mimeType });
            link.download = fileName;

            //add, click and remove
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        });
}

export const setScroll = () => {
    const divMessageContainerBase = document.getElementById("divMessageContainerBase");
    if (divMessageContainerBase != null) {
        divMessageContainerBase.scrollTop = divMessageContainerBase.scrollHeight;
    }
}