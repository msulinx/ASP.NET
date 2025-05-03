const toggle = document.getElementById('darkModeToggle');
const root = document.documentElement;

if (localStorage.getItem('theme') === 'dark') {
    root.setAttribute('data-theme', 'dark');
    toggle.checked = true;
}

toggle.addEventListener('change' , () => {
    if (toggle.checked) {
        root.setAttribute('data-theme', 'dark');
        localStorage.setItem('theme', 'dark');
    } else {
        root.setAttribute('data-theme', 'light');
        localStorage.setItem('theme', 'light');
    }
});