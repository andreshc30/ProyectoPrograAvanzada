document.addEventListener('DOMContentLoaded', function () {
    /**
     * Password Visibility Toggle Logic
     * Enables toggling password input between obscured dots and plain text.
     */
    const togglePasswordButtons = document.querySelectorAll('.password-toggle-btn');

    togglePasswordButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            const inputGroup = this.closest('.campo-clave');
            if (!inputGroup) {
                return;
            }

            const passwordInput = inputGroup.querySelector('.form-control');
            if (!passwordInput) {
                return;
            }

            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);

            const icon = this.querySelector('i');
            if (type === 'text') {
                if (icon) {
                    icon.classList.remove('bi-eye');
                    icon.classList.add('bi-eye-slash');
                }
                this.setAttribute('aria-label', 'Hide password');
            } else {
                if (icon) {
                    icon.classList.remove('bi-eye-slash');
                    icon.classList.add('bi-eye');
                }
                this.setAttribute('aria-label', 'Show password');
            }
        });
    });
    /**
     * Client-Side Form Validation Logic
     * Prevents submission if form inputs are invalid and applies Bootstrap validation styles.
     */
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', function (event) {
            if (!loginForm.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            loginForm.classList.add('was-validated');
        });
    }
});
