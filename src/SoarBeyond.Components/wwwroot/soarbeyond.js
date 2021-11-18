window.soarBeyondRCL = {
    showPrompt: (message) => prompt(message, 'Type anything here')
};

/* Auto-collapse Navbar component on 'nav-link' clicked */
document.addEventListener("click", function (event) {
    const eTarget = event.target;
    const navToggle = document.getElementById("navbarToggler");

    if (eTarget.classList.contains("navbar-toggler")) {
        navToggle.classList.toggle("show");
    } else if (eTarget.classList.contains("nav-link") ||
                eTarget.classList.contains("navbar-brand")) {
        navToggle.classList.remove("show");
    }
});
