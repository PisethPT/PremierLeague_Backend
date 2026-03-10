const modal = document.getElementById("modal");
const backdrop = document.getElementById("modalBackdrop");
const box = document.getElementById("modalBox");

let staticModal = false;

function openModal(sizeClass = "max-w-md", isStatic = false) {
    staticModal = isStatic;

    document.body.style.overflow = "hidden";
    document.body.style.paddingRight = "15px";

    box.className = `
        !bg-[#1e0021] rounded-2xl shadow-xl p-6 transition-all duration-300
        scale-90 opacity-0 w-full max-h-[90vh] overflow-y-auto 
        ${sizeClass}
    `.trim();

    modal.classList.remove("hidden");
    backdrop.classList.remove("hidden");
    box.scrollTop = 0;

    setTimeout(() => {
        box.classList.remove("scale-90", "opacity-0");
        box.classList.add("scale-100", "opacity-100");
    }, 10);
}

function closeModal() {
    box.classList.add("scale-90", "opacity-0");

    document.body.style.overflow = "";
    document.body.style.paddingRight = "";

    setTimeout(() => {
        modal.classList.add("hidden");
        backdrop.classList.add("hidden");
    }, 200);
}

if (backdrop) {
    backdrop.addEventListener("click", (e) => {
        if (!staticModal) closeModal();
    });
}

document.addEventListener("keydown", (e) => {
    if (e.key === "Escape" && !modal.classList.contains("hidden")) {
        if (!staticModal) closeModal();
    }
});