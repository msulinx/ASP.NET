import { initWysiwyg } from '/js/features/wysiwyg.js';
import { initTagSelector } from '/js/components/tags.js';

/* Vissa delar av denna klass är genererade av Chat GPT 4.0 */

document.querySelectorAll('.modal.project-modal').forEach(async modal => {

    // WYSIWYG
    const editor = modal.querySelector('.wysiwyg-editor');
    const toolbar = modal.querySelector('.wysiwyg-toolbar');
    const textarea = modal.querySelector('textarea');

    if (editor && toolbar && textarea) {
        initWysiwyg(editor, toolbar, textarea, textarea.value);
    }

    // Tag Selector
    const tagContainer = modal.querySelector('.form-tag-select');
    const tagInput = modal.querySelector('.form-tag-input');
    const tagResults = modal.querySelector('.search-results');

    if (tagContainer && tagInput && tagResults) {
        try {
            /* Chat GPT --> hämtar alla medlemmar */
            const response = await fetch('/Tags/SearchMembers?term=');
            const allMembers = await response.json();

            /* Chat GPT --> Hämtar redan valda användare (för update) */
            const selectedInputs = modal.querySelectorAll('input[name="SelectedUserIds"]');
            const selectedIds = Array.from(selectedInputs).map(input => input.value);
            
            const preselected = allMembers
                .filter(m => selectedIds.includes(m.id))
                .map(m => ({
                    id: m.id,
                    imageUrl: m.imageUrl,
                    fullName: m.fullName
                }));
            
            initTagSelector({
                containerElement: tagContainer,
                inputElement: tagInput,
                resultsElement: tagResults,
                searchUrl: (query) => `/Tags/SearchMembers?term=${encodeURIComponent(query)}`,
                displayProperty: 'fullName',
                imageProperty: 'imageUrl',
                tagType: 'user',
                tagClass: 'tag',
                emptyMessage: 'No members found',
                avatarFolder: '',
                preselected: preselected
            });
        } catch (e) {
            console.error('Failed to load members', e);
        }
    }
});