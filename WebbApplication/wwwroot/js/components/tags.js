export function initTagSelector(config) {
    let activeIndex = -1;
    let selectedIds = [];

    const tagContainer = config.containerElement;
    const input = config.inputElement;
    const results = config.resultsElement; // CHAT GPT --> För att visa medlemmar i dropdown

    if (Array.isArray(config.preselected)) {
        config.preselected.forEach(item => addTag(item));
    }

    input.addEventListener('focus', () => {
        tagContainer.classList.add('focused');
        results.classList.add('focused');
    });

    input.addEventListener('blur', () => {
        setTimeout(() => {
            tagContainer.classList.remove('focused');
            results.classList.remove('focused');
        }, 100);
    });

    input.addEventListener('input', () => {
        const query = input.value.trim();
        activeIndex = -1;

        if (query.length === 0) {
            results.style.display = 'none';
            results.innerHTML = '';
            return;
        }

        fetch(config.searchUrl(query))
            .then(r => r.json())
            .then(data => renderSearchResults(data));
    });

    function renderSearchResults(data) {
        results.innerHTML = '';

        if (data.length === 0) {
            const noResult = document.createElement('div');
            noResult.classList.add('search-item');
            noResult.textContent = config.emptyMessage || 'No results';
            results.appendChild(noResult);
        } else {
            data.forEach(item => {
                if (!selectedIds.includes(item.id.toString())) {
                    const resultItem = document.createElement('div');
                    resultItem.classList.add('search-item');
                    resultItem.dataset.id = item.id;

                    if (config.tagType === 'tag') {
                        resultItem.innerHTML = `<span>${item[config.displayProperty]}</span>`;
                    } else if (config.tagType === 'user') {
                        resultItem.innerHTML = `
                          <img class="user-avatar" src="${item[config.imageProperty] || ''}" alt="Avatar">
                            <span>${item[config.displayProperty]}</span>`;
                    }

                    resultItem.addEventListener('click', () => addTag(item));
                    results.appendChild(resultItem);
                }
            });
        }

        results.style.display = 'block';
    }

    function addTag(item) {
        const id = item.id.toString();
        if (selectedIds.includes(id)) return;

        selectedIds.push(id);

        const tag = document.createElement('div');
        tag.classList.add(config.tagClass || 'tag');

        if (config.tagType === 'tag') {
            tag.innerHTML = `<span>${item[config.displayProperty]}</span>`;
        } else if (config.tagType === 'user') {
            tag.innerHTML = `
                <img class="user-avatar" src="${item[config.imageProperty] || ''}" alt="Avatar">
                <span>${item[config.displayProperty]}</span>
            `;
        }

        const removeBtn = document.createElement('span');
        removeBtn.classList.add('btn-remove', 'btn-remove-tag');
        removeBtn.dataset.id = id;
        removeBtn.addEventListener('click', (e) => {
            selectedIds = selectedIds.filter(i => i !== id);
            tag.remove();
            updateSelectedIdsInput();
            e.stopPropagation();
        });

        tag.appendChild(removeBtn);
        tagContainer.insertBefore(tag, input);

        input.value = '';
        results.innerHTML = '';
        results.style.display = 'none';

        updateSelectedIdsInput();
    }

    function removeLastTag() {
        const tags = tagContainer.querySelectorAll(`.${config.tagClass || 'tag'}`);
        if (tags.length === 0) return;

        const lastTag = tags[tags.length - 1];
        const lastId = lastTag.querySelector('.btn-remove')?.dataset.id;

        if (!lastId) return;

        selectedIds = selectedIds.filter(id => id !== lastId);
        lastTag.remove();
        updateSelectedIdsInput();
    }

    function updateSelectedIdsInput() {
      
        tagContainer.querySelectorAll('input[name="SelectedUserIds"]').forEach(el => el.remove());

    
        selectedIds.forEach(id => {
            const hiddenInput = document.createElement('input');
            hiddenInput.type = 'hidden';
            hiddenInput.name = 'SelectedUserIds';
            hiddenInput.value = id;
            tagContainer.appendChild(hiddenInput);
        });
    }

    function updateActiveItem(items) {
        items.forEach(item => item.classList.remove('active'));

        if (items[activeIndex]) {
            items[activeIndex].classList.add('active');
            items[activeIndex].scrollIntoView({ block: 'nearest' });
        }
    }

    input.addEventListener('keydown', (e) => {
        const items = results.querySelectorAll('.search-item');

        switch (e.key) {
            case 'ArrowDown':
                e.preventDefault();
                if (items.length > 0) {
                    activeIndex = (activeIndex + 1) % items.length;
                    updateActiveItem(items);
                }
                break;

            case 'ArrowUp':
                e.preventDefault();
                if (items.length > 0) {
                    activeIndex = (activeIndex - 1 + items.length) % items.length;
                    updateActiveItem(items);
                }
                break;

            case 'Enter':
                e.preventDefault();
                if (activeIndex >= 0 && items[activeIndex]) {
                    items[activeIndex].click();
                }
                break;

            case 'Backspace':
                if (input.value === '') {
                    removeLastTag();
                }
                break;
        }
    });
}