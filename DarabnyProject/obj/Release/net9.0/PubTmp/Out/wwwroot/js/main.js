// Smooth Page Transitions without Reload - SPA
let currentView = 'login';

document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM Loaded - Initializing App');
    
    // Check if this is the home page
    const isHomePage = document.querySelector('.hero-section') !== null;
    
    if (isHomePage) {
        console.log('Home page detected');
        loadHomePageScripts();
    } else {
        initializeApp();
        setupEventListeners();
        addAnimationStyles();
        setupDelegatedListeners();

        if (typeof redirectIfAuthenticated === 'function') {
            redirectIfAuthenticated();
        }
    }
});

function setupDelegatedListeners() {
    // Event delegation for dynamically created elements
    document.addEventListener('click', function(e) {
        // Create Account button (في اللوجين)
        if (e.target.classList.contains('Reg-container') && currentView === 'login') {
            console.log('Create Account clicked');
            e.preventDefault();
            switchView('register');
        }
        
        // Sign In button (في التسجيل)
        if (e.target.classList.contains('sign-in-link')) {
            console.log('Sign In link clicked');
            e.preventDefault();
            switchView('login');
        }
        
        // Login/Register button
        if (e.target.classList.contains('login-container')) {
            console.log('Login/Register button clicked');
            e.preventDefault();
            if (currentView === 'login') {
                handleLogin();
            } else {
                handleRegister();
            }
        }
        
        // Social buttons
        if (e.target.closest('.social button')) {
            console.log('Social button clicked');
            e.preventDefault();
            handleSocialLogin(e.target.closest('button'));
        }
        
        // Home button
        if (e.target.classList.contains('HomeBtn')) {
            console.log('Home button clicked');
            e.preventDefault();
            switchView('login');
        }
    });
}

function initializeApp() {
    const userLoggedIn = localStorage.getItem('currentUser');
    currentView = userLoggedIn ? 'dashboard' : 'login';
    showView('login');
}

function setupEventListeners() {
    // Create Account button
    const regContainers = document.querySelectorAll('.Reg-container');
    regContainers.forEach(btn => {
        btn.removeEventListener('click', handleRegisterClick);
        btn.addEventListener('click', handleRegisterClick);
    });
    
    // Login/Register button
    const loginBtn = document.querySelector('.login-container');
    if (loginBtn) {
        loginBtn.removeEventListener('click', handleLoginClick);
        loginBtn.addEventListener('click', handleLoginClick);
    }
    
    // Social buttons
    const socialButtons = document.querySelectorAll('.social button');
    socialButtons.forEach(btn => {
        btn.removeEventListener('click', handleSocialClick);
        btn.addEventListener('click', handleSocialClick);
    });
    
    // Home button
    const homeBtn = document.querySelector('.HomeBtn');
    if (homeBtn) {
        homeBtn.removeEventListener('click', handleHomeClick);
        homeBtn.addEventListener('click', handleHomeClick);
    }
}

function handleRegisterClick(e) {
    e.preventDefault();
    switchView('register');
}

function handleLoginClick(e) {
    e.preventDefault();
    if (currentView === 'login') {
        handleLogin();
    } else {
        handleRegister();
    }
}

function handleSocialClick(e) {
    e.preventDefault();
    handleSocialLogin(this);
}

function handleHomeClick(e) {
    e.preventDefault();
    switchView('login');
}

function addAnimationStyles() {
    const style = document.createElement('style');
    style.textContent = `
        @keyframes slideOut {
            from {
                opacity: 1;
                transform: translateX(0);
            }
            to {
                opacity: 0;
                transform: translateX(-100px);
            }
        }
        
        @keyframes slideIn {
            from {
                opacity: 0;
                transform: translateX(100px);
            }
            to {
                opacity: 1;
                transform: translateX(0);
            }
        }
        
        @keyframes slideInAlert {
            from {
                transform: translateX(400px);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }
        
        @keyframes slideOutAlert {
            from {
                transform: translateX(0);
                opacity: 1;
            }
            to {
                transform: translateX(400px);
                opacity: 0;
            }
        }
        
        .container {
            transition: all 0.5s ease;
        }
        
        .con-right {
            transition: opacity 0.5s ease;
        }
    `;
    document.head.appendChild(style);
}

function switchView(view) {
    console.log('Switching from', currentView, 'to', view);
    
    const conRight = document.querySelector('.con-right');
    if (!conRight) {
        console.error('con-right not found');
        return;
    }
    
    conRight.style.opacity = '0';
    conRight.style.transition = 'opacity 0.3s ease';
    
    setTimeout(() => {
        try {
            if (view === 'login') {
                updateLoginView();
                currentView = 'login';
            } else if (view === 'register') {
                updateRegisterView();
                currentView = 'register';
            }
            
            console.log('View updated to:', currentView);
            conRight.style.opacity = '1';
            setupEventListeners();
            console.log('Event listeners setup complete');
        } catch (error) {
            console.error('Error switching view:', error);
        }
    }, 300);
}

function showView(view) {
    if (view === 'login') {
        updateLoginView();
    } else if (view === 'register') {
        updateRegisterView();
    }
}

function updateLoginView() {
    const conLeft = document.querySelector('.con-left');
    const conRight = document.querySelector('.con-right');
    
    conLeft.innerHTML = `
        <h1>Welcome Back</h1>
        <p>Don't have an account?</p>
        <button class="Reg-container">Create Account</button>
    `;
    
    conRight.innerHTML = `
        <h1>Sign In</h1>
        <div class="input-container">
            <div class="input-group">
                <input type="text" placeholder="Username" id="loginUsername">
            </div>
            <div class="input-group">
                <input type="password" placeholder="Password" id="loginPassword">
            </div>
            <div class="role-selection">
                <label class="radio-option">
                    <input type="radio" name="role" value="student" checked>
                    Student
                </label>
                <label class="radio-option">
                    <input type="radio" name="role" value="teacher">
                    Teacher
                </label>
                    <label class="radio-option">
                        <input type="radio" name="role" value="admin">
                        Admin
                    </label>
                </div>
            <a href="#">Forgot Password?</a>
            <button class="login-container">Sign In</button>
            <p>Or sign up with social media</p>
            <div class="social">
                <button><i class="fa-brands fa-google"></i></button>
                <button><i class="fa-brands fa-facebook-f"></i></button>
                <button><i class="fa-brands fa-linkedin-in"></i></button>
                <button><i class="fa-brands fa-github"></i></button>
            </div>
        </div>
    `;
}

function updateRegisterView() {
    const conLeft = document.querySelector('.con-left');
    const conRight = document.querySelector('.con-right');
    
    conLeft.innerHTML = `
        <h1>Get Started Today</h1>
        <p>Already have an account?</p>
        <button class="sign-in-link">Sign In</button>
    `;
    
    conRight.innerHTML = `
        <h1>Create Account</h1>
        <div class="input-container">
            <div class="input-group">
                <input type="email" placeholder="Email Address" id="registerEmail">
            </div>
            <div class="input-group">
                <input type="text" placeholder="Full Name" id="registerName">
            </div>
            <div class="input-group">
                <input type="password" placeholder="Password" id="registerPassword">
            </div>
            <div class="role-selection">
                <label class="radio-option">
                    <input type="radio" name="role" value="student" checked>
                    Student
                </label>
                <label class="radio-option">
                    <input type="radio" name="role" value="teacher">
                    Teacher
                </label>
                    <label class="radio-option">
                        <input type="radio" name="role" value="admin">
                        Admin
                    </label>
            </div>
            <div class="terms-container">
                <label class="check-item">
                    <input type="checkbox" id="updateCheckbox">
                    <span class="custom-check"></span>
                    Send me updates and latest news
                </label>
                <label class="check-item">
                    <input type="checkbox" id="termsCheckbox">
                    <span class="custom-check"></span>
                    I agree to the <a href="#">Terms of Service</a> and <a href="#">Privacy Policy</a>
                </label>
                <label class="check-item">
                    <input type="checkbox" id="ageCheckbox">
                    <span class="custom-check"></span>
                    I'm over 18 years old
                </label>
            </div>
            <button class="login-container">Register Now</button>
            <p>Or sign up with social media</p>
            <div class="social">
                <button><i class="fa-brands fa-google"></i></button>
                <button><i class="fa-brands fa-facebook-f"></i></button>
                <button><i class="fa-brands fa-linkedin-in"></i></button>
                <button><i class="fa-brands fa-github"></i></button>
            </div>
        </div>
    `;
}

function handleLogin() {
    console.log('handleLogin called');
    const username = document.getElementById('loginUsername') || document.querySelector('.con-right input[type="text"]');
    const password = document.getElementById('loginPassword') || document.querySelector('.con-right input[type="password"]');
    
    if (!username || !password) {
        console.error('Input fields not found');
        showMessage('Error: Input fields not found', 'error');
        return;
    }
    
    const usernameValue = username.value;
    const passwordValue = password.value;
    const role = document.querySelector('input[name="role"]:checked');
    const roleValue = role ? role.value : 'student';
    
    console.log('Login attempt:', { usernameValue, passwordValue, roleValue });
    
    if (!usernameValue || !passwordValue) {
        showMessage('Please fill in all fields', 'error');
        return;
    }
    
    if (passwordValue.length < 3) {
        showMessage('Password must be at least 3 characters', 'error');
        return;
    }
    
    const userData = {
        username: usernameValue,
        role: roleValue,
        loginTime: new Date().toLocaleString()
    };
    
    localStorage.setItem('currentUser', JSON.stringify(userData));
    console.log('User data saved:', userData);
    
    showMessage('Login successful! Welcome ' + usernameValue, 'success');
    
    setTimeout(() => {
        const defaultRedirect = roleValue === 'admin' ? '/Dashboard/Admin' : '/Dashboard/Student';
        const redirectTarget = typeof getPostLoginRedirect === 'function'
            ? getPostLoginRedirect(defaultRedirect)
            : defaultRedirect;

        if (typeof clearPostLoginRedirect === 'function') {
            clearPostLoginRedirect();
        }

        console.log('Redirecting to', redirectTarget);
        window.location.href = redirectTarget;
    }, 1500);
}

function handleRegister() {
    const email = document.getElementById('registerEmail')?.value;
    const fullName = document.getElementById('registerName')?.value;
    const password = document.getElementById('registerPassword')?.value;
    const role = document.querySelector('input[name="role"]:checked')?.value;
    const termsAgreed = document.getElementById('termsCheckbox')?.checked;
    const ageConfirmed = document.getElementById('ageCheckbox')?.checked;
    
    if (!email || !fullName || !password) {
        showMessage('Please fill in all required fields', 'error');
        return;
    }
    
    if (!email.includes('@')) {
        showMessage('Please enter a valid email address', 'error');
        return;
    }
    
    if (password.length < 6) {
        showMessage('Password must be at least 6 characters', 'error');
        return;
    }
    
    if (!termsAgreed || !ageConfirmed) {
        showMessage('Please agree to all terms and conditions', 'error');
        return;
    }
    
    const userData = {
        email: email,
        fullName: fullName,
        role: role,
        registrationTime: new Date().toLocaleString()
    };
    
    localStorage.setItem('newUser', JSON.stringify(userData));
    showMessage('Registration successful! Switching to Sign In...', 'success');
    setTimeout(() => {
        switchView('login');
    }, 1500);
}

function handleSocialLogin(button) {
    const iconClass = button.querySelector('i').className;
    let platform = '';
    
    if (iconClass.includes('google')) {
        platform = 'Google';
    } else if (iconClass.includes('facebook')) {
        platform = 'Facebook';
    } else if (iconClass.includes('linkedin')) {
        platform = 'LinkedIn';
    } else if (iconClass.includes('github')) {
        platform = 'GitHub';
    }
    
    showMessage('Connecting to ' + platform + '...', 'info');
}

function showMessage(message, type) {
    const messageDiv = document.createElement('div');
    messageDiv.className = `message-alert message-${type}`;
    messageDiv.textContent = message;
    
    messageDiv.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 15px 25px;
        border-radius: 8px;
        color: white;
        font-weight: 600;
        z-index: 1000;
        animation: slideInAlert 0.3s ease;
        max-width: 300px;
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    `;
    
    if (type === 'success') {
        messageDiv.style.backgroundColor = '#4CAF50';
    } else if (type === 'error') {
        messageDiv.style.backgroundColor = '#f44336';
    } else if (type === 'info') {
        messageDiv.style.backgroundColor = '#2196F3';
    }
    
    document.body.appendChild(messageDiv);
    
    setTimeout(() => {
        messageDiv.style.animation = 'slideOutAlert 0.3s ease';
        setTimeout(() => {
            messageDiv.remove();
        }, 300);
    }, 3000);
}

// ==================== HOME PAGE FUNCTIONS ====================

function loadHomePageScripts() {
    console.log('Loading Home Page Scripts');

    // Home page UX interactions
    setupFaqAccordion();
    setupScrollReveal();

    if (typeof syncAuthNav === 'function') {
        syncAuthNav();
    }
}

function setupFaqAccordion() {
    const faqItems = document.querySelectorAll('.faq-item');
    if (!faqItems.length) {
        return;
    }

    faqItems.forEach(item => {
        const questionBtn = item.querySelector('.faq-question');
        if (!questionBtn) {
            return;
        }

        questionBtn.addEventListener('click', () => {
            const isActive = item.classList.contains('active');

            faqItems.forEach(otherItem => {
                otherItem.classList.remove('active');
                const otherBtn = otherItem.querySelector('.faq-question');
                if (otherBtn) {
                    otherBtn.setAttribute('aria-expanded', 'false');
                }
            });

            if (!isActive) {
                item.classList.add('active');
                questionBtn.setAttribute('aria-expanded', 'true');
            }
        });
    });
}

function setupScrollReveal() {
    const revealTargets = document.querySelectorAll('.reveal-on-scroll');
    if (!revealTargets.length) {
        return;
    }

    const revealObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (!entry.isIntersecting) {
                return;
            }

            entry.target.classList.add('reveal-active');
            observer.unobserve(entry.target);
        });
    }, {
        threshold: 0.2
    });

    revealTargets.forEach(target => {
        revealObserver.observe(target);
    });
}



