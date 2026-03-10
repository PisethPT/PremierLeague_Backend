const VIDEO_BASE_CONTROLLER = "/en/videos";
const VIDEO_ENDPOINT = {
  CREATE: VIDEO_BASE_CONTROLLER + "/create",
  UPDATE: VIDEO_BASE_CONTROLLER + "/update",
  GET: VIDEO_BASE_CONTROLLER + "/get-video",
};

(async () => {
  const { CustomSelect } = await import("/js/shared/select_custom.js");
  window.CustomSelect = CustomSelect;

  window.selectCategoryInst = CustomSelect.init(
    document.getElementById("selectVideoCategory"),
    { showImage: false, placeholder: "Select Category" },
  );
})();

function updateCharCounter(input) {
  const display = document.querySelector(`[data-count-for="${input.id}"]`);
  if (!display) return;
  const length = input.value.length;
  display.textContent = length;
}

document.addEventListener("DOMContentLoaded", function () {
  const trackedInputs = document.querySelectorAll(".char-counter-input");
  trackedInputs.forEach((input) => {
    updateCharCounter(input);
    input.addEventListener("input", () => updateCharCounter(input));
  });
});

const resetForm = () => {
  let form = $("#videoForm");
  form[0].reset();
  form.find("input[type='hidden']").val("");
  window.selectCategoryInst.setValue("");

  // Refresh counters
  document
    .querySelectorAll(".char-counter-input")
    .forEach((el) => updateCharCounter(el));
};

$("#btnAddVideo").on("click", function () {
  const form = $("#videoForm");
  form.attr("action", VIDEO_ENDPOINT.CREATE);
  resetForm();
  $("#modalTitle").text("Create Video");

  const today = new Date().toISOString().split("T")[0];
  $("#publishedDate").val(today);

  const expiry = new Date();
  expiry.setFullYear(expiry.getFullYear() + 1);
  $("#expiryDate").val(expiry.toISOString().split("T")[0]);

  openModal("modal-8xl", true);
});

function toggleEditVideo(videoId) {
  resetForm();

  $.ajax({
    url: VIDEO_ENDPOINT.GET + "/" + videoId,
    method: "GET",
    headers: {
      RequestVerificationToken: $(
        'input[name="__RequestVerificationToken"]',
      ).val(),
    },
    success: function (response) {
      if (response.statusCode !== 200) {
        alert(response.message);
        return;
      }

      const data = response.data.videoItem;
      const form = $("#videoForm");

      $("#modalTitle").text("Edit Video");
      form.attr("action", VIDEO_ENDPOINT.UPDATE + "/" + videoId);

      form.find("#videoId").val(data.videoId);
      form.find("#title").val(data.title);
      form.find("#description").val(data.description);
      form.find("#videoUrl").val(data.videoUrl);
      form.find("#channel").val(data.channel);
      form.find("#publisher").val(data.publisher);

      if (data.publishedDate) {
        form.find("#publishedDate").val(data.publishedDate.split("T")[0]);
      }
      if (data.expiryDate) {
        form.find("#expiryDate").val(data.expiryDate.split("T")[0]);
      }

      if (window.selectCategoryInst) {
        window.selectCategoryInst.setValue(data.videoCategoryId);
      }
      form.find("#isFeatured").prop("checked", data.isFeatured);
      form.find("#isActive").prop("checked", data.isActive);

      document
        .querySelectorAll(".char-counter-input")
        .forEach((el) => updateCharCounter(el));
      openModal("modal-8xl", true);
    },
    error: function (xhr) {
      console.error("Error fetching video:", xhr);
    },
  });
}
