document.addEventListener("DOMContentLoaded", () => {
    if (!getCookie("cookieConsent")) 
        showCookieModal()
})

function showCookieModal() {
    const modal = document.getElementById('cookieModal')
    if (modal) modal.style.display = "flex"

    const consentValue = getCookie("cookieConsent")
    if (!consentValue) return

    try {
        const consent = JSON.parse(consentValue)
        document.getElementById('cookieFunctional').checked = consent.functional
        document.getElementById('cookieAnalytics').checked = consent.analytics
        document.getElementById('cookieMarketing').checked = consent.marketing
    }
    catch (error) {
        console.error('Unable to handle cookie consent values', error);
    }
}

function hideCookieModal() {
    const modal = document.getElementById('cookieModal')
    if (modal) modal.style.display = "none";
}

console.log("cookieConsent (raw):", getCookie("cookieConsent"));

function getCookie(name) {
    const nameEQ = name + "="
    const cookies = document.cookie.split(";");
    for (let cookie of cookies) {
        cookie = cookie.trim()
        if (cookie.indexOf(nameEQ) === 0) {
            return decodeURIComponent(cookie.substring(nameEQ.length))
        }
    }
    
    return null
} 


async function acceptAll() {
    const consent = {
        essential: true,
        functional: true,
        analytics: true,
        marketing: true
    }
    await handleConsent(consent)
    hideCookieModal()
}

async function acceptSelected() {
    const form = document.getElementById('cookieConsentForm');
    const formData = new FormData(form);
    
    const consent = {
        essential: true,
        functional: formData.get("functional") === "on",
        analytics: formData.get("analytics") === "on",
        marketing: formData.get("marketing") === "on"
    }
    
    await handleConsent(consent)
    hideCookieModal()
}

async function handleConsent(consent) {
    try {
        const res = await fetch('/cookies/setcookies', {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(consent)
        })
        
        if (!res.ok) {
            console.error('Unable to set cookie consent', await res.text())
        }
    }
    catch (error) {
        console.error("Error::", error);
    }
}