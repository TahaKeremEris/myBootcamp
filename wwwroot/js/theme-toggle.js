const themeToggleBtn = document.getElementById("themeToggle");
const themeLink = document.getElementById("theme-link");

function setButtonText(isDark) {
    if (themeToggleBtn) {
        themeToggleBtn.innerHTML = isDark ? "☀️ Light Mode" : "🌙 Dark Mode";
    }
}

function updateNavLinkColors(isDark) {
    const navLinks = document.querySelectorAll(".theme-sensitive");
    navLinks.forEach(link => {
        link.classList.remove("text-dark", "text-light");
        link.classList.add(isDark ? "text-light" : "text-dark");
    });
}

function loadTheme() {
    const savedTheme = localStorage.getItem("theme");
    const isDark = savedTheme === "dark";

    themeLink.href = isDark
        ? "https://cdn.jsdelivr.net/npm/bootstrap-dark-5@1.1.3/dist/css/bootstrap-dark.min.css"
        : "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css";

    setButtonText(isDark);
    updateNavLinkColors(isDark);
}

function toggleTheme() {
    const isDark = themeLink.href.includes("bootstrap-dark");

    if (isDark) {
        themeLink.href = "https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css";
        localStorage.setItem("theme", "light");
    } else {
        themeLink.href = "https://cdn.jsdelivr.net/npm/bootstrap-dark-5@1.1.3/dist/css/bootstrap-dark.min.css";
        localStorage.setItem("theme", "dark");
    }

    setButtonText(!isDark);
    updateNavLinkColors(!isDark);
}

document.addEventListener("DOMContentLoaded", function () {
    loadTheme();
    if (themeToggleBtn) {
        themeToggleBtn.addEventListener("click", toggleTheme);
    }
});
