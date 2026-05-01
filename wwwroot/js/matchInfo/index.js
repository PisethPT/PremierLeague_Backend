const MATCH_INFO_BASE_CONTROLLER = "/en/match-info";
const MATCH_INFO_ENDPOINT = {
  CREATE_MATCH_INFO_ENDPOINT: MATCH_INFO_BASE_CONTROLLER + "/create",
  UPDATE_MATCH_INFO_ENDPOINT: MATCH_INFO_BASE_CONTROLLER + "/update",
  DELETE_MATCH_INFO_ENDPOINT: MATCH_INFO_BASE_CONTROLLER + "/delete",
  GET_MATCH_INFO_ENDPOINT: MATCH_INFO_BASE_CONTROLLER + "/get-match-info",
  GET_MATCH_ITEMS_ENDPOINT: MATCH_INFO_BASE_CONTROLLER + "/get-match",
};

(async () => {
  const { MatchSelect } = await import("/js/shared/match_select.js");

  window.MatchSelect = MatchSelect;

  window.selectMatchInst = MatchSelect.init(
    document.getElementById("selectMatch"),
    {
      showImage: true,
      placeholder: "Select Match",
      imgSize: "w-auto h-7",
    },
  );
})();

const resetForm = () => {
  let form = $("#matchInfoForm");
  form[0].reset();

  form.find("input").not("[name='__RequestVerificationToken']").val("");

  if (window.selectMatchInst) {
    window.selectMatchInst.reset();
  }
};

(function () {
  $("#resetBtn").on("click", resetForm);
})();

$("#btnAddNewMatchInfo").on("click", function () {
  resetForm();
  getMatchItems(false);
  const form = $("#matchInfoForm");
  form.attr("action", MATCH_INFO_ENDPOINT.CREATE_MATCH_INFO_ENDPOINT);
  openModal("modal-8xl", true);
});

function toggleEdit(matchInfoId) {
  resetForm();
  try {
    $.ajax({
      url: MATCH_INFO_ENDPOINT.GET_MATCH_INFO_ENDPOINT + "/" + matchInfoId,
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
        const data = response.data.item;

        const form = $("#matchInfoForm");
        form.find("#matchInfoId").val(data.matchInfoId);
        form.find("#selectMatch").val(data.matchId);
        form.find("#attendance").val(data.attendance);
        form.find("#weather").val(data.weather);
        form.find("#pitchCondition").val(data.pitchCondition);
        form.find("#addedTimeFirstHalf").val(data.addedTimeFirstHalf);
        form.find("#addedTimeSecondHalf").val(data.addedTimeSecondHalf);

        getMatchItems(true, data.matchId);

        form.attr(
          "action",
          MATCH_INFO_ENDPOINT.UPDATE_MATCH_INFO_ENDPOINT + "/" + matchInfoId,
        );
        $("#modalTitle").text("Update Match Info");
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

function getMatchItems(isEdit = false, existingMatchId = "") {
  try {
    $.ajax({
      url: MATCH_INFO_ENDPOINT.GET_MATCH_ITEMS_ENDPOINT + "/" + isEdit,
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
        const data = response.data.item;

        if (window.selectMatchInst && data) {
          window.selectMatchInst.setData(data);

          if (existingMatchId) {
            window.selectMatchInst.setValue(existingMatchId);
            form.find("#selectMatch").val(existingMatchId);
          }
        } else {
          console.error("Match selection failed: Instance or data missing", {
            instance: window.selectMatchInst,
            data: data,
          });
        }
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
