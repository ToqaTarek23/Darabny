(function() {
    const THEME_KEY = 'darabny-theme';

    function getInitialTheme() {
        const storedTheme = localStorage.getItem(THEME_KEY);
        if (storedTheme === 'light' || storedTheme === 'dark') {
            return storedTheme;
        }

        const systemPrefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
        return systemPrefersDark ? 'dark' : 'light';
    }

    function applyTheme(theme) {
        document.body.setAttribute('data-theme', theme);
        localStorage.setItem(THEME_KEY, theme);
    }

    function createThemeToggleButton(isModernNavbar) {
        const button = document.createElement('button');
        button.type = 'button';
        button.className = isModernNavbar
            ? 'login-chip theme-toggle-btn border-0'
            : 'my-2 my-lg-0 link-btn rounded-5 py-2 px-3 border-0 theme-toggle-btn';
        button.setAttribute('id', 'themeToggleBtn');
        button.setAttribute('aria-label', 'Toggle theme');
        return button;
    }

    function updateThemeToggleButton(button, theme) {
        if (!button) {
            return;
        }

        const isDark = theme === 'dark';
        button.innerHTML = isDark
            ? '<span class="theme-toggle-icon" aria-hidden="true">☀️</span>'
            : '<span class="theme-toggle-icon" aria-hidden="true">🌙</span>';
        button.setAttribute('aria-pressed', isDark ? 'true' : 'false');
        button.setAttribute('title', isDark ? 'Switch to light mode' : 'Switch to dark mode');
    }

    function mountThemeToggleButton(theme) {
        const isModernNavbar = Boolean(document.querySelector('.page-navbar'));
        const actionsContainer = document.querySelector('.page-navbar .navbar-collapse .d-flex.align-items-center, .navbar .navbar-collapse .ms-auto.d-flex.align-items-center');
        const collapseContainer = document.querySelector('.page-navbar .navbar-collapse, .navbar .navbar-collapse');
        const target = actionsContainer || collapseContainer;

        if (!target || document.getElementById('themeToggleBtn')) {
            return;
        }

        const button = createThemeToggleButton(isModernNavbar);
        updateThemeToggleButton(button, theme);

        button.addEventListener('click', function() {
            const currentTheme = document.body.getAttribute('data-theme') === 'dark' ? 'dark' : 'light';
            const nextTheme = currentTheme === 'dark' ? 'light' : 'dark';

            applyTheme(nextTheme);
            updateThemeToggleButton(button, nextTheme);
        });

        target.appendChild(button);
    }

    document.addEventListener('DOMContentLoaded', function() {
        if (!document.querySelector('.page-navbar, .navbar')) {
            return;
        }

        const theme = getInitialTheme();
        applyTheme(theme);
        mountThemeToggleButton(theme);
    });
})();
