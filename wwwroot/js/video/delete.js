const $backdrop = $("#modalBackdrop");
let isStaticModal = false;

function showModal(modalId, button, sizeClass = "modal-md", isStatic = false) {
    isStaticModal = !!isStatic;
    const $modal = $("#" + modalId);
    const $box = $modal.find(".modalBox");

    const videoId = $(button).find('input[name="videoId"]').val();
    const row = $(button).closest('tr');
    const title = row.find('td:nth-child(5) div').attr('title');
    const category = row.find('td:nth-child(7)').text();
    const thumbUrl = row.find('td:nth-child(4) img').attr('src');

    $modal.find("#deleteVideoId").val(videoId);
    $modal.find("#videoTitleDisplay").text(title);
    $modal.find("#videoCategoryDisplay").text(category);
    $modal.find("#videoThumbDisplay").attr("src", thumbUrl);

    document.body.style.overflow = "hidden";

    $box.attr("class", `modalBox !bg-[#1e0021] rounded-2xl shadow-xl transition-all duration-300 scale-90 opacity-0 w-full ${sizeClass}`.trim());

    $modal.removeClass("hidden");
    $backdrop.removeClass("hidden");

    setTimeout(() => {
        $box.removeClass("scale-90 opacity-0").addClass("scale-100 opacity-100");
    }, 10);
}

function hideModal(modalId) {
    const $modal = modalId ? $("#" + modalId) : $(".modal").not(".hidden").last();
    if (!$modal.length) return;

    const $box = $modal.find(".modalBox");
    $box.removeClass("scale-100 opacity-100").addClass("scale-90 opacity-0");

    document.body.style.overflow = "";

    setTimeout(() => {
        $modal.addClass("hidden");
        $backdrop.addClass("hidden");
        isStaticModal = false;
    }, 200);
}

$backdrop.on("click", () => {
    if (!isStaticModal) hideModal();
});