var dropdownToggle = document.querySelectorAll('.dropdown-toggle');
document.addEventListener("DOMContentLoaded", function () {
    dropdownToggle.forEach(function (toggle) {
        toggle.addEventListener('click', function () {
            var dropdown = this.parentElement.querySelector('.dropdown-menu');
            dropdown.classList.toggle('show');
        });
    });

    dropdownToggle.addEventListener('click', function (event) {
        var dropdowns = document.querySelectorAll('.dropdown-menu');
        dropdowns.forEach(function (dropdown) {
            if (!dropdown.contains(event.target)) {
                dropdown.classList.remove('show');
            }
        });
    });
});
