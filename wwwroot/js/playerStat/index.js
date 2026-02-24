const PLAYER_STAT_BASE_CONTROLLER = "/player-stats";
const PLAYER_STAT_ENDPOINT = {
  CREATE_PLAYER_STAT_ENDPOINT: PLAYER_STAT_BASE_CONTROLLER + "/create",
  UPDATE_PLAYER_STAT_ENDPOINT: PLAYER_STAT_BASE_CONTROLLER + "/update",
  FIND_PLAYER_STAT_BY_ID_ENDPOINT: PLAYER_STAT_BASE_CONTROLLER + "/get-match",
  GET_STAT_ITEMS_ENDPOINT: PLAYER_STAT_BASE_CONTROLLER + "/get-stats",
  GET_PLAYER_STATS_ENDPOINT: PLAYER_STAT_BASE_CONTROLLER + "/get-players",
};

let playerItems = [];

const resetForm = () => {
  let form = $("#playerStatForm");
  form[0].reset();
  form.find("input").not("[name='__RequestVerificationToken']").val("");

  form.find("select.js-custom-select").each(function () {
    const placeholder = $(this).data("placeholder") || "Please select";
    const $btn = $(this).parent().find("button");
    $btn.find("img").addClass("hidden");
    $btn.find("span.truncate.block").text(placeholder);
  });

  $("#previewSection").removeClass("flex").addClass("hidden");
};

(function () {
  $("#resetBtn").on("click", resetForm);
  validateForm("#playerStatForm");
})();

(async () => {
  const { CustomSelect } = await import("/js/shared/select_custom.js");

  window.CustomSelect = CustomSelect;

  window.statCategoryInst = CustomSelect.init(
    document.getElementById("statCategory"),
    {
      showImage: false,
      placeholder: "Empty Stat Category",
    },
  );
  window.statInst = CustomSelect.init(document.getElementById("stat"), {
    showImage: false,
    placeholder: "Empty Stats",
  });

  window.playerInst = CustomSelect.init(document.getElementById("player"), {
    showImage: false,
    placeholder: "Empty Player",
  });
})();

function validateForm(formSelector) {
  $(formSelector).on("submit", function (e) {
    e.preventDefault();
    const form = this;
    let isValid = true;

    if (isValid && !form.checkValidity()) {
      form.reportValidity();
      return false;
    }
    if (isValid) form.submit();
  });
}

function toggleAddStat(matchId, clubId, isHomeClub = true) {
  resetForm();
  openModal("modal-8xl", true);

  $("#statCategory").off("change");
  $("#statCategory").on("change", function () {
    const selectedId = $(this).val();

    window.playerInst.updateOptions([]);
    window.playerInst.setValue("");
    $("#previewSection").removeClass("flex").addClass("hidden");

    if (!selectedId) {
      window.statInst.updateOptions([]);
      window.statInst.setValue("");
      return;
    }

    $.ajax({
      url: PLAYER_STAT_ENDPOINT.GET_STAT_ITEMS_ENDPOINT + `/${selectedId}`,
      type: "GET",
      headers: {
        RequestVerificationToken: $(
          'input[name="__RequestVerificationToken"]',
        ).val(),
      },
      success: function (response) {
        const { statsSelectListItem } = response.data;
        const statArray = JSON.stringify(statsSelectListItem);
        window.statInst.updateOptions(JSON.parse(statArray));

        $("#stat").off("change");
        $("#stat").on("change", function () {
          const selected = $(this).val();
          $("#previewSection").removeClass("flex").addClass("hidden");

          if (!selected) {
            window.playerInst.updateOptions([]);
            window.playerInst.setValue("");
            return;
          }

          $.ajax({
            url:
              PLAYER_STAT_ENDPOINT.GET_PLAYER_STATS_ENDPOINT +
              `/${matchId}/${clubId}`,
            method: "GET",
            headers: {
              RequestVerificationToken: $(
                'input[name="__RequestVerificationToken"]',
              ).val(),
            },
            data: { statId: selected, isHomeClub: isHomeClub },
            success: function (response) {
              const defaultPlayerPath = "/upload/players/";
              const { isPlayerStat, isClubStat, symbol } = response.data;
              playerItems = response.data.playerItems;

              $("#valueLabelId").text(
                "Value (" + (symbol ? symbol : "N/A") + ")",
              );

              if (!Array.isArray(playerItems)) {
                playerItems = [playerItems];
              }

              const itemOptions = playerItems.map((it) => ({
                value: it.playerId,
                label: `${it.firstName || ""} ${it.lastName || ""}`.trim(),
                img: defaultPlayerPath + (it.photo || "placeholder.png"),
                subtitle: `${it.position || ""} • ${it.playerNumber ?? ""}`,
              }));

              window.playerInst.updateOptions(itemOptions);
            },
            error: function (err) {
              console.error(err);
            },
          });
        });
      },
      error: function (err) {
        console.error(err);
      },
    });
  });
}

function toggleAccordion(header) {
  const content = header.nextElementSibling;
  const icon = header.querySelector("i");

  if (content.style.maxHeight) {
    content.style.maxHeight = null;
    icon.classList.remove("rotate-180");
  } else {
    content.style.maxHeight = content.scrollHeight + "px";
    icon.classList.add("rotate-180");
  }
}

$("#player").on("change", function () {
  $("#previewSection").removeClass("flex").addClass("hidden");
  const selectedPlayerId = $(this).val();
  const selectedPlayer = playerItems.find(
    (player) => player.playerId == selectedPlayerId,
  );

  if (selectedPlayer) {
    $("#previewSection").removeClass("hidden").addClass("flex");
    $("#clubCrest").attr(
      "src",
      selectedPlayer.clubCrest
        ? `/upload/clubs/${selectedPlayer.clubCrest}`
        : "/upload/clubs/placeholder.png",
    );
    $("#playerPhoto").attr(
      "src",
      selectedPlayer.photo
        ? `/upload/players/${selectedPlayer.photo}`
        : "/upload/players/placeholder.png",
    );
    $("#playerFirstName").text(selectedPlayer.firstName || "");
    $("#playerLastName").text(selectedPlayer.lastName || "");
    $("#playerPosition").text(selectedPlayer.position || "");
    $("#playerNumber").text(
      selectedPlayer.playerNumber ? `#${selectedPlayer.playerNumber}` : "",
    );
    $("#clubTheme").css(
      "background-color",
      selectedPlayer.clubColor || "#ff0000",
    );
  }
});
