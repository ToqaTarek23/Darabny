// Auth helpers — works with ASP.NET Identity cookies (server-side)
// localStorage auth is removed; Identity cookie handles everything

function logoutUser() {
    // Calls server-side Logout which clears the Identity cookie
    window.location.href = '/Account/Logout';
}

// Sync navbar display based on server-rendered data attribute
function syncAuthNav() {
    var isAuth = document.body.dataset.authenticated === 'true';
    var userName = document.body.dataset.username || '';

    var loginLink  = document.getElementById('homeLoginLink');
    var logoutBtn  = document.getElementById('homeLogoutBtn');
    var userLabel  = document.getElementById('adminUserLabel');

    if (loginLink)  loginLink.style.display  = isAuth ? 'none'  : '';
    if (logoutBtn)  logoutBtn.style.display  = isAuth ? ''      : 'none';
    if (userLabel && isAuth) {
        userLabel.style.display = '';
        userLabel.textContent   = userName;
    }
}

document.addEventListener('DOMContentLoaded', syncAuthNav);
