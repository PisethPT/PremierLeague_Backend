const $backdrop = $("#modalBackdrop");
let isStaticModal = false;
let textColor = "white";

function showModal(modalId, button, sizeClass = "modal-md", isStatic = false) {
  isStaticModal = !!isStatic;
  const $modal = $("#" + modalId);
  const $box = $modal.find(".modalBox");
  const $newsId = $(button).find('input[name="newsId"]').val();
  const $title = $(button).find('input[name="title"]').val();
  const $content = $(button).find('input[name="content"]').val();
  const $image = $(button).find('input[name="image"]').val();

  const imageUrl = "/upload/news/" + $image;
  $modal.find("#newsId").val($newsId);
  $modal.find("#imageUrlDisplay").attr("src", imageUrl);
  $modal.find("#titleDisplay").text($title);
  $modal.find("#contentDisplay").text($content);

  $box.attr(
    "class",
    `
      modalBox
      !bg-[#1e0021] rounded-2xl shadow-xl p-6 transition-all duration-300
      scale-90 opacity-0 h-auto w-full ${sizeClass}
    `.trim(),
  );

  $modal.removeClass("hidden");
  $backdrop.removeClass("hidden");

  setTimeout(() => {
    $box.removeClass("scale-90 opacity-0");
    $box.addClass("scale-100 opacity-100");
  }, 10);
}

function hideModal(modalId) {
  if (isStaticModal && typeof modalId === "undefined") return;

  let $modal, $box;

  if (modalId) {
    $modal = $("#" + modalId);
  } else {
    $modal = $(".modal").not(".hidden").last();
  }

  if (!$modal || $modal.length === 0) return;

  $box = $modal.find(".modalBox");
  $box.removeClass("scale-100 opacity-100");
  $box.addClass("scale-90 opacity-0");

  setTimeout(() => {
    $modal.addClass("hidden");
    $backdrop.addClass("hidden");
    isStaticModal = false;
  }, 200);
}

if ($backdrop && $backdrop.length) {
  $backdrop.on("click", () => {
    if (!isStaticModal) hideModal();
  });
}
