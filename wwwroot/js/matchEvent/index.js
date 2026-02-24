const MATCH_EVENTS_BASE_CONTROLLER = "/match-events";
const MATCH_EVENTS_ENDPOINT = {
  CREATE_MATCH_EVENTS_ENDPOINT: MATCH_EVENTS_BASE_CONTROLLER + "/create",
  UPDATE_MATCH_EVENTS_ENDPOINT: MATCH_EVENTS_BASE_CONTROLLER + "/update",
  FIND_MATCH_EVENTS_BY_ID_ENDPOINT: MATCH_EVENTS_BASE_CONTROLLER + "/get-match",
  GET_PLAYER_ENDPOINT: MATCH_EVENTS_BASE_CONTROLLER + "/get-players",
};

(async () => {
  const { CustomSelect } = await import("/js/shared/select_custom.js");

  window.CustomSelect = CustomSelect;

  window.eventTypeInst = CustomSelect.init(
    document.getElementById("eventTypeSelect"),
    {
      showImage: false,
      placeholder: "Event Type",
    },
  );

  window.outcomeInst = CustomSelect.init(
    document.getElementById("outcomeSelect"),
    {
      showImage: false,
      placeholder: "Outcome",
    },
  );

  window.playerInst = CustomSelect.init(
    document.getElementById("playerSelect"),
    {
      showImage: true,
      placeholder: "Empty Player",
    },
  );
  window.relatedEventInst = CustomSelect.init(
    document.getElementById("relatedEventSelect"),
    {
      showImage: true,
      placeholder: "Related Event",
    },
  );
})();

let playerItems = [];

const resetForm = () => {
  let form = $("#matchEventForm");
  form[0].reset();
  form.find("input").not("[name='__RequestVerificationToken']").val("");

  form.find("select.js-custom-select").each(function () {
    const placeholder = $(this).data("placeholder") || "Please select";
    const $btn = $(this).parent().find("button");
    $btn.find("img").addClass("hidden");
    $btn.find("span.truncate.block").text(placeholder);
  });

  form.find("input[type='checkbox']").prop("checked", false);
  form.find("input[type='checkbox']").closest("div").find("span").text("No");

  $("#isPenaltyHidden").val("false");
  $("#isOwnGoalHidden").val("false");
  $("#isInsideBoxHidden").val("false");
  $("#isBigChanceHidden").val("false");
  $("#isWoodworkHidden").val("false");

  $("#previewSection").removeClass("flex").addClass("hidden");
};

(function () {
  $("#resetBtn").on("click", resetForm);
  validateForm("#matchEventForm");
})();

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

function toggleAddMatchEvent(matchId, clubId, isHomeClub = true) {
  resetForm();
  openModal("modal-8xl", true);

  $("#matchId").val(matchId);
  $("#clubId").val(clubId);

  $.ajax({
    url: MATCH_EVENTS_ENDPOINT.GET_PLAYER_ENDPOINT + `/${matchId}/${clubId}`,
    method: "GET",
    headers: {
      RequestVerificationToken: $(
        'input[name="__RequestVerificationToken"]',
      ).val(),
    },
    data: { isHomeClub: isHomeClub },
    success: function (response) {
      const defaultPlayerPath = "/upload/players/";
      playerItems = response.data.playerItems;

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
}

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

function toggleRelatedEvent(playerId) {
  const relatedEventSelect = $("#relatedEventSelect");
  let relatedEventItems = [];
  const defaultPlayerPath = "/upload/players/";
  if (playerId)
    playerItems.map((player) => {
      if (player.playerId != playerId) {
        relatedEventItems.push({
          value: player.playerId,
          label: `${player.firstName || ""} ${player.lastName || ""}`.trim(),
          img: defaultPlayerPath + (player.photo || "placeholder.png"),
          subtitle: `${player.position || ""} • ${player.playerNumber ?? ""}`,
        });
      }
    });
  else relatedEventItems = playerItems;

  relatedEventSelect.empty();
  window.relatedEventInst.updateOptions(relatedEventItems);
  relatedEventSelect.val(null).trigger("change");
}

$("#playerSelect").on("change", function () {
  toggleRelatedEvent($(this).val());
});

$("#playerSelect").on("change", function () {
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

$("#isPenalty").on("change", function () {
  $("#isPenaltyLabel").text(this.checked ? "Yes" : "No");
  $("#isPenaltyHidden").val(this.checked ? "true" : "false");
});

$("#isOwnGoal").on("change", function () {
  $("#isOwnGoalLabel").text(this.checked ? "Yes" : "No");
  $("#isOwnGoalHidden").val(this.checked ? "true" : "false");
});

$("#isInsideBox").on("change", function () {
  $("#isInsideBoxLabel").text(this.checked ? "Yes" : "No");
  $("#isInsideBoxHidden").val(this.checked ? "true" : "false");
});

$("#isBigChance").on("change", function () {
  $("#isBigChanceLabel").text(this.checked ? "Yes" : "No");
  $("#isBigChanceHidden").val(this.checked ? "true" : "false");
});

$("#isWoodwork").on("change", function () {
  $("#isWoodworkLabel").text(this.checked ? "Yes" : "No");
  $("#isWoodworkHidden").val(this.checked ? "true" : "false");
});
