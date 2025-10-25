const LOGO_PATH = `${window.location.origin}/images/motorcycle_logo.png?v=${new Date().getTime()}`;
const DOC_LABEL = "Documentation";
const API_NAME = "RENTAL API";
const VERSION_LABEL = "Version";

function customizeTopbar() {
    const link = document.querySelector('.topbar-wrapper a');
    if (!link) return;

    link.innerHTML = `
        <div style="display:flex;align-items:center;gap:10px;">
            <img src="${LOGO_PATH}" alt="${API_NAME}"
                 style="height:40px;width:auto;vertical-align:middle;filter:brightness(0.9);" />
            <div style="display:flex;flex-direction:column;line-height:1.2;">
                <span style="font-size:11px;color:#fff;font-weight:600;margin-bottom:2px;">${DOC_LABEL}</span>
                <span style="color:#fff;font-weight:700;font-size:20px;font-family:Poppins;">${API_NAME}</span>
            </div>
        </div>
    `;
}

function customizeVersionLabel() {
    const label = document.querySelector('.download-url-wrapper .select-label span');
    if (label) {
        label.textContent = VERSION_LABEL;
    }
}

function startObserver() {
    const observer = new MutationObserver(() => {
        const topbar = document.querySelector('.topbar-wrapper a img');
        const version = document.querySelector('.download-url-wrapper .select-label span');

        if (topbar && !topbar.src.includes("motorcycle_logo.png")) {
            customizeTopbar();
        }
        if (version && version.textContent !== VERSION_LABEL) {
            customizeVersionLabel();
        }
    });

    observer.observe(document.body, { childList: true, subtree: true });

    customizeTopbar();
    customizeVersionLabel();
}

function waitForSwaggerReady(callback, retries = 0) {
    const target = document.querySelector('.topbar-wrapper a');

    if (target) {
        console.log("Swagger DOM detected");
        callback();
        return;
    }

    if (retries > 60) { 
        console.warn("Swagger UI did not mount in time.");
        return;
    }

    requestAnimationFrame(() => waitForSwaggerReady(callback, retries + 1));
}


window.addEventListener("load", function () {
    const preloadImage = new Image();
    preloadImage.src = LOGO_PATH;

    preloadImage.onload = function () {
        waitForSwaggerReady(() => {
            startObserver();
            console.log("Swagger UI customization applied successfully!");
        });
    };
});


