export function initWysiwyg(editorEl, toolbarEl, textareaEl, content) {
    const quill = new Quill(editorEl, {
        modules: {
            syntax: true,
            toolbar: toolbarEl
        },
        placeholder: 'Type something',
        theme: 'snow'
    });

    if (content) {
        quill.root.innerHTML = content;
    }

    quill.on('text-change', () => {
        textareaEl.value = quill.root.innerHTML;
    });
}