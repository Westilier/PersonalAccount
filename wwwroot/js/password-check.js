document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector("#passwordForm");
    const oldPassword = document.querySelector("#oldPassword");
    const password = document.querySelector("#newPassword");
    const confirm = document.querySelector("#confirmPassword");
    const newPasswordErrorSpan = document.querySelector("#newPasswordError");
    const confirmErrorSpan = document.querySelector("#confirmError");

    if (!form || !oldPassword || !password || !confirm || !newPasswordErrorSpan || !confirmErrorSpan) return;

    function checkPasswords() {
        let isValid = true;

        if (oldPassword.value === password.value) {
            newPasswordErrorSpan.textContent = "Старый и новый пароль совпадают";
            isValid = false;
        } else {
            newPasswordErrorSpan.textContent = "";
        }
        if (password.value !== confirm.value) {
            confirmErrorSpan.textContent = "Новый пароль и пароль подтверждения не совпадают";
            isValid = false;
        } else {
            confirmErrorSpan.textContent = "";
        }

        return isValid;
    }

    oldPassword.addEventListener("input", checkPasswords);
    password.addEventListener("input", checkPasswords);
    confirm.addEventListener("input", checkPasswords);

    form.addEventListener("submit", (event) =>{
        if (!checkPasswords()) {
            event.preventDefault();
        }
    });
});