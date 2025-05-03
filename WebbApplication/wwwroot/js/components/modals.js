// Öppna modal
document.querySelectorAll('[data-type="modal"]').forEach(modal => {
    modal.addEventListener('click', function () {
        const targetId = modal.getAttribute('data-target');
        const targetElement = document.querySelector(targetId);
        targetElement.classList.add('modal-show');
    });
});

// Stäng modal
document.querySelectorAll('[data-type="close"]').forEach(button => {
    button.addEventListener('click', function () {
        const targetId = button.getAttribute('data-target');
        const targetElement = document.querySelector(targetId);
        targetElement.classList.remove('modal-show');
    });
});

// Forms

document.addEventListener('submit', async (e) => {
    const form = e.target;

    if (!form.matches('form')) return;
    if (form.classList.contains('logout-form')) return;

    e.preventDefault();

    clearErrorMessages(form);

    const formData = new FormData(form);

    try {
        const res = await fetch(form.action, {
            method: 'post',
            body: formData
        });

        if (res.redirected) {
            window.location.href = res.url;
            return;
        }

        if (res.ok) {
            const openModal = form.closest('.modal');
            if (openModal) openModal.classList.remove('modal-show');
            location.reload();
            return;
        }

        if (res.status === 400) {
            const data = await res.json();
            if (data.errors) {
                Object.keys(data.errors).forEach(key => {
                    addErrorMessage(key, data.errors[key].join('\n'));
                });
            }
        }
    } catch {
        console.log('error submitting the form');
    }
});

// Select dropdown
document.querySelectorAll('.form-select').forEach(select => {
    const trigger = select.querySelector('.form-select-trigger');
    const text = select.querySelector('.form-select-text');
    const options = select.querySelectorAll('.form-select-option');
    const hiddenInput = select.querySelector('input[type="hidden"]');
    const placeholder = select.dataset.placeholder || 'Choose';

    const selectedOption = select.querySelector('.form-select-option.selected');
    if (selectedOption) {
        text.textContent = selectedOption.textContent.trim();
        hiddenInput.value = selectedOption.getAttribute('data-value');
    } else {
        text.textContent = placeholder;
    }

    trigger.addEventListener('click', () => {
        select.classList.toggle('show');
    });

    options.forEach(option => {
        option.addEventListener('click', () => {
            const value = option.dataset.value;
            const label = option.textContent;

            text.textContent = label;
            hiddenInput.value = value;
            select.classList.remove('show');
        });
    });

    document.addEventListener('click', (e) => {
        if (!select.contains(e.target)) {
            select.classList.remove('show');
        }
    });
});

// Image preview
document.querySelectorAll('.upload-trigger').forEach(trigger => {
    const modal = trigger.closest('.modal');
    const fileInput = modal.querySelector('.image-upload');
    const preview = modal.querySelector('.image-preview');
    const previewIconContainer = modal.querySelector('.image-preview-icon-container');
    const previewIcon = modal.querySelector('.image-preview-icon');

    trigger.addEventListener('click', () => fileInput.click());

    fileInput.addEventListener('change', (e) => {
        const file = e.target.files[0];
        if (file && file.type.startsWith('image/')) {
            const reader = new FileReader();
            reader.onload = () => {
                preview.src = reader.result;
                preview.classList.remove('hide');
                previewIconContainer.classList.add('selected');
                previewIcon.classList.remove('fa-camera');
                previewIcon.classList.add('fa-pen');
                previewIcon.classList.add('moved');
                
            };
            reader.readAsDataURL(file);
        }
    });
});

function clearErrorMessages(form) {
    const errorSpans = form.querySelectorAll('.text-danger');
    errorSpans.forEach(span => span.textContent = '');
}

function addErrorMessage(fieldName, errorMessage) {
    const input = document.querySelector(`[name="${fieldName}"]`);
    if (!input) return;

    const errorSpan = input.closest('.form-group')?.querySelector('.text-danger');
    if (errorSpan) {
        errorSpan.textContent = errorMessage;
    } else {
        const span = document.createElement('span');
        span.className = 'text-danger';
        span.textContent = errorMessage;
        input.insertAdjacentElement('afterend', span);
    }
}