const MATCH_EVENTS_BASE_CONTROLLER = "/en/match-events";
const MATCH_EVENTS_ENDPOINT = {
  CREATE_MATCH_EVENTS_ENDPOINT: MATCH_EVENTS_BASE_CONTROLLER + "/create",
  UPDATE_MATCH_EVENTS_ENDPOINT: MATCH_EVENTS_BASE_CONTROLLER + "/update",
  GET_MATCH_EVENT_ENDPOINT: MATCH_EVENTS_BASE_CONTROLLER + "/get-match-event",
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
  const form = $("#matchEventForm");
  form.attr("action", MATCH_EVENTS_ENDPOINT.CREATE_MATCH_EVENTS_ENDPOINT);
  resetForm();
  openModal("modal-8xl", true);

  $("#matchId").val(matchId);
  $("#clubId").val(clubId);
  toggleGetPlayers(matchId, clubId, isHomeClub);
}

async function toggleEditMatchEvent(matchEventId, isHomeClub = true) {
  try {
    resetForm();

    const response = await $.ajax({
      url: MATCH_EVENTS_ENDPOINT.GET_MATCH_EVENT_ENDPOINT + `/${matchEventId}`,
      method: "GET",
      headers: {
        RequestVerificationToken: $(
          'input[name="__RequestVerificationToken"]',
        ).val(),
      },
    });

    const data = response.data.matchEvent;

    $("#matchEventId").val(data.matchEventId);
    $("#matchId").val(data.matchId);
    $("#clubId").val(data.clubId);
    eventTypeInst.setValue(data.eventTypeId);
    outcomeInst.setValue(data.outcomeId);

    $("#minuteId").val(data.minute);
    $("#isPenalty").prop("checked", data.isPenalty).trigger("change");
    $("#isOwnGoal").prop("checked", data.isOwnGoal).trigger("change");
    $("#isInsideBox").prop("checked", data.isInsideBox).trigger("change");
    $("#isBigChance").prop("checked", data.isBigChance).trigger("change");
    $("#isWoodwork").prop("checked", data.isWoodwork).trigger("change");

    await toggleGetPlayers(data.matchId, data.clubId, isHomeClub);
    playerInst.setValue(data.playerId);
    toggleRelatedEvent(data.playerId);
    relatedEventInst.setValue(data.relatedEventId);

    $("#modalTitle").text("Update Match Event");
    $("#matchEventForm").attr(
      "action",
      MATCH_EVENTS_ENDPOINT.UPDATE_MATCH_EVENTS_ENDPOINT +
        "/" +
        data.matchEventId,
    );

    openModal("modal-8xl", true);
  } catch (err) {
    console.error(err);
    alert(JSON.stringify(err));
  }
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

function toggleGetPlayers(matchId, clubId, isHomeClub = true) {
  return $.ajax({
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
  });
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
