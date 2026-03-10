const NEWS_BASE_CONTROLLER = "/en/news";
const NEWS_ENDPOINT = {
  CREATE_NEWS_ENDPOINT: NEWS_BASE_CONTROLLER + "/create",
  UPDATE_NEWS_ENDPOINT: NEWS_BASE_CONTROLLER + "/update",
  DELETE_NEWS_ENDPOINT: NEWS_BASE_CONTROLLER + "/delete",
  GET_NEWS_ENDPOINT: NEWS_BASE_CONTROLLER + "/get-news",
};

(async () => {
  const { CustomSelect } = await import("/js/shared/select_custom.js");

  window.CustomSelect = CustomSelect;

  window.selectNewsTagInst = CustomSelect.init(
    document.getElementById("selectNewsTag"),
    {
      showImage: false,
      placeholder: "Select News Tag",
    },
  );

  // window.selectMatchInst = CustomSelect.init(
  //   document.getElementById("selectMatch"),
  //   {
  //     showImage: false,
  //     placeholder: "Select Match",
  //   },
  // );
})();

function updateCharCounter(input) {
  const display = document.querySelector(`[data-count-for="${input.id}"]`);
  if (!display) return;

  const length = input.value.length;
  const max = input.getAttribute("maxlength");

  display.textContent = length;

  if (length >= max * 0.95) {
    display.parentElement.classList.replace("text-gray-400", "text-orange-400");
  } else {
    display.parentElement.classList.replace("text-orange-400", "text-gray-400");
  }
}

document.addEventListener("DOMContentLoaded", function () {
  const trackedInputs = document.querySelectorAll(".char-counter-input");

  trackedInputs.forEach((input) => {
    updateCharCounter(input);
    input.addEventListener("input", () => updateCharCounter(input));
  });
});
$("#removePhoto").on("click", function () {
  resetFile();
});

const resetForm = () => {
  let form = $("#newsForm");
  form[0].reset();
  resetFile();

  form.find("input").not("[name='__RequestVerificationToken']").val("");
  form.find("textarea").val("");

  form.find("select.js-custom-select").each(function () {
    const placeholder = $(this).data("placeholder") || "Please select";
    const $btn = $(this).parent().find("button");
    $btn.find("img").addClass("hidden");
    $btn.find("span.truncate.block").text(placeholder);
  });

  $("#newsForm")
    .find("input[type='file']")
    .each(function () {
      $(this).attr("required", true);
    });

  $("#newsForm").find("span[asp-validation-for='NewsDto.ImageUrl']").text("");
  document
    .querySelectorAll(".char-counter-input")
    .forEach((el) => updateCharCounter(el));
};

$("#btnAddNews").on("click", function () {
  const form = $("#newsForm");
  form.attr("action", NEWS_ENDPOINT.CREATE_NEWS_ENDPOINT);
  resetForm();
  const today = new Date();
  $("#publishedDate").val(today.toISOString().split("T")[0]);

  const expiry = new Date();
  expiry.setDate(expiry.getDate() + 7);
  $("#expiryDate").val(expiry.toISOString().split("T")[0]);
  openModal("modal-8xl", true);
});

function toggleEditNews(newsId) {
  resetForm();
  try {
    $.ajax({
      url: NEWS_ENDPOINT.GET_NEWS_ENDPOINT + "/" + newsId,
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
        const data = response.data.newsItem;
        const form = $("#newsForm");
        form.find("#newsId").val(data.newsId);
        form.find("#title").val(data.title);
        form.find("#subtitle").val(data.subtitle);
        form.find("#content").val(data.content);

        // Manually update counters for specific elements without events
        document
          .querySelectorAll(".char-counter-input")
          .forEach((el) => updateCharCounter(el));
        const publishedDate = new Date(data.publishedDate);
        const expiryDate = new Date(data.expiryDate);
        form
          .find("#publishedDate")
          .val(publishedDate.toISOString().split("T")[0]);
        form.find("#expiryDate").val(expiryDate.toISOString().split("T")[0]);
        window.selectNewsTagInst.setValue(data.newsTagId);
        form.find("#imageUrl").val(data.imageUrl);
        //window.selectMatchInst.setValue(data.matchId);

        if (data.imageUrl) {
          const photoPath = "/upload/news/" + data.imageUrl;
          $("#filePreview").attr("src", photoPath);
          $("#fileName").text(data.imageUrl);
          $("#previewArea").removeClass("hidden");
          $("#fileInput").attr("required", false);
        } else {
          $("#previewArea").addClass("hidden");
          $("#filePreview").attr("src", "");
          $("#fileName").text("");
          $("#fileInput").attr("required", true);
        }

        form.attr("action", NEWS_ENDPOINT.UPDATE_NEWS_ENDPOINT + "/" + newsId);
        openModal("modal-8xl", true);
      },
      error: function (xhr, status, error) {
        console.error(error);
        alert(JSON.stringify(error));
      },
    });
  } catch (error) {
    console.error(error);
    alert(JSON.stringify(error));
  }
}
