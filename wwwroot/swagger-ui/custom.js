
window.addEventListener("load", function () {
    const addGouthamText = () => {
        // Look for the "MyApi" title element
        const titleElement = document.querySelector(".swagger-ui .title");

        if (titleElement && !document.getElementById("goutham-text")) {
            const span = document.createElement("span");
            span.id = "goutham-text";
            span.innerHTML = ' &nbsp;&nbsp;<strong>I am Goutham — <a href="https://www.google.com" target="_blank">Go to Google</a></strong>';
            span.style.fontSize = "18px";
            span.style.fontWeight = "500";
            span.style.color = "#1a73e8"; // Google blue

            titleElement.appendChild(span);
            console.log("✅ Goutham text added next to MyApi title.");
        } else {
            // Retry until Swagger UI finishes loading
            setTimeout(addGouthamText, 500);
        }
    };

    addGouthamText();
});