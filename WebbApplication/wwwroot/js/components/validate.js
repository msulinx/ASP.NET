const validateField = (field) => {
    let errorSpan = document.querySelector(`span[data-valmsg-for="${field.name}"]`);
    if (!errorSpan) return;

    let errorMessage = "";
    let value = field.value.trim();

    if (field.hasAttribute("data-val-required")) {
        if (value === "") {
            errorMessage = field.getAttribute("data-val-required");
        }
    }

    if (!errorMessage && field.hasAttribute("data-val-regex") && value !== "") {
        let pattern = new RegExp(field.getAttribute("data-val-regex-pattern"));
        if (!pattern.test(value)) {
            errorMessage = field.getAttribute("data-val-regex");
        }
    }

    if (errorMessage) {
        field.classList.add("input-validation-error");
        errorSpan.classList.remove("field-validation-valid");
        errorSpan.classList.add("field-validation-error");
        errorSpan.textContent = errorMessage;
    } else {
        field.classList.remove("input-validation-error");
        errorSpan.classList.remove("field-validation-error");
        errorSpan.classList.add("field-validation-valid");
        errorSpan.textContent = "";
    }
}

// Denna kod är konstruerat av Chat GPT för att validera all innehåll i modalerna på sidan
const validateForm = (form) => {
    let isValid = true;
    const fields = form.querySelectorAll('input[data-val="true"], textarea[data-val="true"], select[data-val="true"]');

    fields.forEach(field => {
        validateField(field);

        if (field.classList.contains('input-validation-error')) {
            isValid = false;
        }
    });

    return isValid;
};

document.addEventListener('input', function(event) {
    const field = event.target;

    if (field.matches('input[data-val="true"], textarea[data-val="true"], select[data-val="true"]')) {
        validateField(field);
    }
});