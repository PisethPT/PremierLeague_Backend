const CLUB_BASE_CONTROLLER = "/en/clubs";
const CLUB_ENDPOINT = {
  CREATE_CLUB_ENDPOINT: CLUB_BASE_CONTROLLER + "/create",
  UPDATE_CLUB_ENDPOINT: CLUB_BASE_CONTROLLER + "/update",
  FIND_CLUB_BY_ID_ENDPOINT: CLUB_BASE_CONTROLLER + "/get-club",
};

(function () {
  const form = document.getElementById("clubForm");
  const removeLogo = document.getElementById("removeLogo");

  removeLogo.addEventListener("click", () => {
    resetFile();
  });

  document.getElementById("resetBtn").addEventListener("click", () => {
    form.reset();
    form.querySelectorAll("input").forEach(
      (input) =>
        function () {
          if (input.name != "__RequestVerificationToken") input.value = "";
        },
    );
    resetFile();
    resetColorPicker();
  });

  document.getElementById("clubForm").addEventListener("submit", (e) => {
    const form = e.target;

    if (!form.checkValidity()) {
      form.reportValidity();
      return;
    }

    const clubId = document.getElementById("clubId").value;
    const logoInput = document.getElementById("fileInput");
    const logoError = document.getElementById("fileError");

    logoError.classList.add("hidden");
    logoError.textContent = "";

    const isCreate = clubId === "" || clubId === "0";
    const hasNewFile = logoInput.files.length > 0;

    if (isCreate && !hasNewFile) {
      logoError.classList.remove("hidden");
      logoError.textContent = "Please upload a club logo.";
      e.preventDefault();
      return;
    }
  });
})();

document.getElementById("btnAddNewClub").addEventListener("click", (e) => {
  const clubForm = document.getElementById("clubForm");
  $("#modalTitle").text("Create New Club");
  clubForm.reset();
  resetFile();
  clubForm.querySelectorAll("input").forEach(
    (input) =>
      function () {
        if (input.name != "__RequestVerificationToken") input.value = "";
      },
  );

  $("#isAllTimePremierLeagueClub").prop("checked", false);
  $("#isAllTimePremierLeagueClubHidden").val("false");

  resetColorPicker();
  $("#clubForm").attr("action", CLUB_ENDPOINT.CREATE_CLUB_ENDPOINT);
});

function attachUpdateClub(clubId) {
  $.ajax({
    url: CLUB_ENDPOINT.FIND_CLUB_BY_ID_ENDPOINT + "/" + clubId,
    method: "POST",
    headers: {
      RequestVerificationToken: $(
        'input[name="__RequestVerificationToken"]',
      ).val(),
    },
    success: function (data) {
      $("#clubId").val(data.id);
      $("#clubName").val(data.name ?? "");
      $("#clubShortName").val(data.clubNameThirdChar ?? "");
      $("#founded").val(data.founded ?? "");
      $("#city").val(data.city ?? "");
      $("#stadium").val(data.stadium ?? "");
      $("#coach").val(data.headCoach ?? "");
      $("#website").val(data.clubOfficialWebsite ?? "");
      $("#crest").val(data.crest ?? "");

      $("#isAllTimePremierLeagueClub").prop(
        "checked",
        data.isAllTimePremierLeagueClub,
      );
      $("#isAllTimePremierLeagueClubHidden").val(
        data.isAllTimePremierLeagueClub ? "true" : "false",
      );

      if (data.theme) {
        $("#themeColor").val(data.theme);
        $("#themeHex").val(data.theme);
      } else {
        $("#themeColor").val("#55005a");
        $("#themeHex").val("#55005a");
      }

      if (data.crest) {
        const logoPath = "/upload/clubs/" + data.crest;
        $("#filePreview").attr("src", logoPath);
        $("#fileName").text(data.crest);
        $("#previewArea").removeClass("hidden");
        $("#fileInput").attr("required", false);
      } else {
        $("#previewArea").addClass("hidden");
        $("#filePreview").attr("src", "");
        $("#fileName").text("");
        $("#fileInput").attr("required", true);
      }

      $("#modalTitle").text("Update Club");
      $("#clubForm").attr(
        "action",
        CLUB_ENDPOINT.UPDATE_CLUB_ENDPOINT + "/" + data.id,
      );

      openModal("modal-8xl", true);
    },
    error: function (err) {
      alert(JSON.stringify(err));
      console.error(err);
    },
  });
}

$("#isAllTimePremierLeagueClub").on("change", function () {
  $("#isAllTimePremierLeagueClubHidden").val(this.checked ? "true" : "false");
});
