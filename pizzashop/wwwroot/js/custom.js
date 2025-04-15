const passwordFields = document.querySelectorAll(".password-field");
const passwordToggleIcons = document.querySelectorAll(".password-toggle-icon i");
passwordToggleIcons.forEach((toggleIcon, index) => {
    toggleIcon.addEventListener("click", () => {
        const passwordField = passwordFields[index];

        if (passwordField.type === "password") {
            passwordField.type = "text";
            toggleIcon.classList.remove("fa-eye-slash");
            toggleIcon.classList.add("fa-eye");
        } else {
            passwordField.type = "password";
            toggleIcon.classList.remove("fa-eye");
            toggleIcon.classList.add("fa-eye-slash");
        }
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.getElementById("sidebar");
    const toggleButton = document.getElementById("toggleSidebar");

    function toggleSidebar() {
        const isOpen = sidebar.classList.contains("open");
        if (isOpen) {
            closeSidebar();
        } else {
            openSidebar();
        }
    }

    function openSidebar() {
        sidebar.classList.add("open");
        sidebar.style.transform = "translateX(0%)";
    }

    function closeSidebar() {
        sidebar.classList.remove("open");
        sidebar.style.transform = "translateX(-100%)";
    }

    toggleButton.addEventListener("click", toggleSidebar);
    function handleResize() {
        if (window.innerWidth >= 1100) {
            openSidebar();
        } else {
            closeSidebar();
        }
    }

    window.addEventListener("resize", handleResize);
    if (window.innerWidth < 1100) {
        closeSidebar();
    } else {
        openSidebar();
    }
});
function fileUploaded(){
    const FileInput=document.getElementById("FileInput");
    const cloudButton=document.getElementById("cloudButton");
    cloudButton.click=
        FileInput.click();
}

function checkSession() {
    $.ajax({
        url: "/Auth/CheckSession",
        type: "GET",
        success: function(response) {
            console.log("Session Active:", response.message);
        },
        error: function(xhr) {
            if (xhr.status === 401) {
                alert("Session expired! Redirecting to login...");
                window.location.href = "/Auth/Login";
            }
        }
    });
}

document.addEventListener('DOMContentLoaded', function() {
    const sidebarItems = document.querySelectorAll('.sidebar > ul > li');
    const activeItem = localStorage.getItem('activeMenuItem');
    if (activeItem) {
        sidebarItems.forEach(item => {
            const link = item.querySelector('a');
            if (link && link.getAttribute('href') === activeItem) {
                item.classList.add('active');
            }
        });
    }
    
    sidebarItems.forEach(item => {
        item.addEventListener('click', function(e) {
            const link = this.querySelector('a');
            if (link) {
                localStorage.setItem('activeMenuItem', link.getAttribute('href'));
                sidebarItems.forEach(i => i.classList.remove('active'));
                this.classList.add('active');
            }
        });
    });

    const currentPath = window.location.pathname;
    sidebarItems.forEach(item => {
        const link = item.querySelector('a');
        if (link && link.getAttribute('href') === currentPath) {
            item.classList.add('active');
            localStorage.setItem('activeMenuItem', currentPath);
        }
    });

    const toggleButton = document.getElementById('toggleSidebar');
    const sidebar = document.querySelector('.sidebar');
    
    if (toggleButton && sidebar) {
        toggleButton.addEventListener('click', function() {
            if (window.innerWidth < 800) {
                if (sidebar.style.transform === 'translateX(0px)') {
                    sidebar.style.transform = 'translateX(-100%)';
                } else {
                    sidebar.style.transform = 'translateX(0)';
                }
            }
        });
    }

    window.addEventListener('resize', function() {
        if (window.innerWidth >= 800) {
            sidebar.style.transform = 'translateX(0%)';
        } else {
            sidebar.style.transform = 'translateX(-100%)';
        }
    });
});


