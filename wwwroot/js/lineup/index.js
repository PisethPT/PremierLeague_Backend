const LINEUP_BASE_CONTROLLER = "/en/lineups";
const LINEUP_ENDPOINT = {
  CREATE_LINEUP_ENDPOINT: LINEUP_BASE_CONTROLLER + "/create",
  UPDATE_LINEUP_ENDPOINT: LINEUP_BASE_CONTROLLER + "/update",
  FIND_LINEUP_BY_ID_ENDPOINT: LINEUP_BASE_CONTROLLER + "/get-lineup",
  FIND_FORMATION_ENDPOINT: LINEUP_BASE_CONTROLLER + "/get-formations",
  GET_PLAYERS_BY_MATCH_ID_ENDPOINT:
    LINEUP_BASE_CONTROLLER + "/get-players-by-match",
  GET_LINEUP_CLUB_INFO_BY_MATCH_ID_ENDPOINT:
    LINEUP_BASE_CONTROLLER + "/get-lineup-club-info-by-match",
  GET_LINEUP_FORMATION_DETAIL_BY_MATCH_ID_ENDPOINT:
    LINEUP_BASE_CONTROLLER + "/get-lineup-formation-detail-by-match",
};

const BASE_CLUB_PATH = "/upload/clubs/";
const BASE_PLAYER_PATH = "/upload/players/";
const tabBtns = document.querySelectorAll(".tab-btn");
const tabContents = document.querySelectorAll(".tab-content");

tabBtns.forEach((btn) => {
  btn.addEventListener("click", () => {
    tabBtns.forEach((b) => {
      b.classList.remove("text-[#8a3fbf]", "border-b-2", "border-[#8a3fbf]");
      b.classList.add("text-gray-400");
    });

    tabContents.forEach((c) => c.classList.add("hidden"));

    btn.classList.add("text-[#8a3fbf]", "border-b-2", "border-[#8a3fbf]");
    btn.classList.remove("text-gray-400");

    document.getElementById(btn.dataset.tab).classList.remove("hidden");
  });
});

$("#btnAddNewLineup").on("click", function () {});

(async () => {
  const { MatchSelect } = await import("/js/shared/match_select.js");
  const { CustomSelect } = await import("/js/shared/select_custom.js");

  window.MatchSelect = MatchSelect;
  window.CustomSelect = CustomSelect;

  window.matchSelectInst = MatchSelect.init(
    document.getElementById("matchSelectId"),
    {
      showImage: true,
      placeholder: "Select Match",
      imgSize: "w-auto h-7",
    },
  );

  window.homeClubFormationInst = CustomSelect.init(
    document.getElementById("homeClubFormation"),
    {
      showImage: false,
      placeholder: "",
    },
  );

  window.awayClubFormationInst = CustomSelect.init(
    document.getElementById("awayClubFormation"),
    {
      showImage: false,
      placeholder: "",
    },
  );
})();

$("#matchSelectId").on("change", function () {
  const matchId = $(this).val();
  if (!matchId) return;

  $.ajax({
    url: LINEUP_ENDPOINT.GET_PLAYERS_BY_MATCH_ID_ENDPOINT + `/${matchId}`,
    headers: {
      RequestVerificationToken: $(
        'input[name="__RequestVerificationToken"]',
      ).val(),
    },
    method: "GET",
    success: function (response) {
      if (response.statusCode !== 200) return;

      renderPlayers({
        containerId: "#homeClubPlayerId",
        players: response.data.homePlayers,
        side: "home",
      });

      renderPlayers({
        containerId: "#awayClubPlayerId",
        players: response.data.awayPlayers,
        side: "away",
      });

      renderFormation("home", 1);
      renderFormation("away", 1);

      slotHomePlayers = getPlayerIds("homeClubPlayerId");
      slotAwayPlayers = getPlayerIds("awayClubPlayerId");

      state.home.slotPlayers = [...slotHomePlayers];
      state.away.slotPlayers = [...slotAwayPlayers];

      renderBench("home");
      renderBench("away");
    },
    error: function (err) {
      console.error("Error fetching players:", err);
    },
  });
});

async function viewLineupFormation(matchId) {
  try {
    const response = await $.ajax({
      url:
        LINEUP_ENDPOINT.GET_LINEUP_CLUB_INFO_BY_MATCH_ID_ENDPOINT +
        `/${matchId}`,
      method: "GET",
      headers: {
        RequestVerificationToken: $(
          'input[name="__RequestVerificationToken"]',
        ).val(),
      },
    });

    const lineupClubInfoData = response.data.lineupClubInfo;
    $("#homeClubCrest").attr(
      "src",
      BASE_CLUB_PATH + lineupClubInfoData.homeClubCrest,
    );

    $("#awayClubCrest").attr(
      "src",
      BASE_CLUB_PATH + lineupClubInfoData.awayClubCrest,
    );

    $("#homeClubTheme").css(
      "background-color",
      lineupClubInfoData.homeClubTheme,
    );

    $("#awayClubTheme").css(
      "background-color",
      lineupClubInfoData.awayClubTheme,
    );

    $("#homeClubName").text(lineupClubInfoData.homeClubName);
    $("#awayClubName").text(lineupClubInfoData.awayClubName);

    $("#homeClubFormationTitle").text(lineupClubInfoData.homeClubFormation);
    $("#awayClubFormationTitle").text(lineupClubInfoData.awayClubFormation);
    $("#homeClubManager").text(lineupClubInfoData.homeClubManager);
    $("#awayClubManager").text(lineupClubInfoData.awayClubManager);

    const detailResponse = await getLineupFormationDetail(matchId);
    const { lineupFormation, substitutionFormation } = detailResponse.data;

    renderTeam(
      "homePitch",
      lineupClubInfoData.homeClubFormation,
      lineupFormation,
    );
    renderTeam(
      "awayPitch",
      lineupClubInfoData.awayClubFormation,
      lineupFormation,
    );

    renderSubstitutes(
      "homeSubstitutes",
      substitutionFormation.filter((p) => p.isHomeClub),
    );
    renderSubstitutes(
      "awaySubstitutes",
      substitutionFormation.filter((p) => !p.isHomeClub),
    );
  } catch (err) {
    console.error(err);
    alert(JSON.stringify(err));
  }
}

function getLineupFormationDetail(matchId) {
  return $.ajax({
    url:
      LINEUP_ENDPOINT.GET_LINEUP_FORMATION_DETAIL_BY_MATCH_ID_ENDPOINT +
      `/${matchId}`,
    method: "GET",
    headers: {
      RequestVerificationToken: $(
        'input[name="__RequestVerificationToken"]',
      ).val(),
    },
  });
}

function renderPlayers({ containerId, players, side }) {
  const $container = $(containerId);
  $container.empty();

  playerStore[side] = {};

  if (!players || players.length === 0) {
    $container.html(`<p class="text-gray-400 text-sm">No players found</p>`);
    return;
  }

  let html = "";

  players.forEach((player) => {
    const normalizedPlayer = {
      id: player.playerId,
      firstName: player.firstName,
      lastName: player.lastName,
      position: player.position,
      positionId: Number(player.positionId),
      playerNumber: player.playerNumber,
      img: `/upload/players/${player.photo}`,
      clubTheme: player.clubTheme,
    };

    playerStore[side][normalizedPlayer.id] = normalizedPlayer;

    html += `
      <div
        data-player-id="${normalizedPlayer.id}"
        class="flex items-center gap-3 bg-[#1e0021] p-2 rounded-2xl cursor-grab hover:ring-2 hover:ring-[#8a3fbf]"
      >
        <div class="w-14 h-14 rounded-2xl overflow-hidden flex justify-center items-center"
             style="background-color:${normalizedPlayer.clubTheme}">
          <img src="${normalizedPlayer.img}"
               class="h-[4rem] mt-5 object-contain"
               draggable="true"
               ondragstart="onPlayerDrag(event, '${side}')" />
        </div>

        <div>
          <p class="text-sm text-white font-medium">${normalizedPlayer.firstName}</p>
          <p class="text-xs text-white font-medium">${normalizedPlayer.lastName}</p>
          <p class="text-xs text-gray-300 mt-2">
            ${normalizedPlayer.position} • #${normalizedPlayer.playerNumber}
          </p>
        </div>
      </div>
    `;
  });

  $container.html(html);
}

function renderTeam(teamId, formationString, allPlayers) {
  const container = document.getElementById(teamId);
  const isHome = teamId === "homePitch";

  const teamPlayers = allPlayers
    .filter((p) => p.isHomeClub === isHome)
    .sort((a, b) => a.formationSlot - b.formationSlot);

  const layout = [1, ...formationString.split("-").map(Number)];

  container.innerHTML = "";
  if (isHome) {
    container.classList.replace("flex-col-reverse", "flex-col");
  } else {
    container.classList.replace("flex-col", "flex-col-reverse");
  }

  let playerPointer = 0;
  layout.forEach((playerCount, rowIndex) => {
    const rowPlayers = teamPlayers.slice(
      playerPointer,
      playerPointer + playerCount,
    );
    createRow(container, rowPlayers, layout, rowIndex);
    playerPointer += playerCount;
  });
}

function createRow(container, rowPlayers, layout, rowIndex) {
  const row = document.createElement("div");
  row.className = "flex justify-around items-center w-full min-h-[60px]";

  const isHome = container.id === "homePitch";
  const isGKRow = rowIndex === 0;

  rowPlayers.forEach((player) => {
    const isGK = rowIndex === 0;
    const bgColor = isGK ? "#fde047" : player.clubTheme;
    row.innerHTML += `
        <div class="player-node flex flex-col items-center group cursor-pointer hover:scale-110 transition-transform duration-300">
            <div class="relative">
                
                <div class="absolute -top-1 -right-2 flex flex-col gap-1 z-20">
                    ${player.hasYellowCard ? '<div class="w-2 h-3 bg-yellow-400 rounded-sm border border-black/20 shadow-sm"></div>' : ""}
                    ${player.hasGoal ? '<div class="bg-white rounded-full p-0.5 shadow-sm border border-gray-200"><svg class="w-2.5 h-2.5" viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8z"/></svg></div>' : ""}
                </div>

                ${player.subOutMinute ? `<span class="absolute -top-4 -left-2 text-[10px] font-black text-white italic drop-shadow-md">${player.subOutMinute}' <span class="text-red-500">↓</span></span>` : ""}

                <div class="w-12 h-12 md:w-14 md:h-14 rounded-lg shadow-2xl flex items-end justify-center overflow-hidden" 
                     style="background: #37003c">
                    <img src="${BASE_PLAYER_PATH + player.playerPhoto}" 
                         class="w-[90%] h-[90%] object-contain drop-shadow-lg" 
                         onerror="this.src='/upload/players/placeholder.png'">
                </div>

                ${player.isCaptain ? '<div class="absolute bottom-4 -left-1 bg-white text-black text-[7px] font-black px-1 rounded-sm border border-black/20 shadow-sm">C</div>' : ""}
            </div>

            <div class="mt-1 flex flex-col items-center">
                <div class="flex items-center gap-1">
                    <span class="text-[10px] text-white/80 font-bold">${player.playerNumber}</span>
                    <p class="text-[10px] text-white">
                        ${player.playerShortName}
                    </p>
                </div>
            </div>
        </div>`;
  });
  container.appendChild(row);
}

function renderSubstitutes(containerId, players) {
  const container = document.getElementById(containerId);
  const isAway = containerId === "awaySubstitutes";
  container.innerHTML = "";

  if (!players || players.length === 0) {
    container.innerHTML =
      '<p class="text-[10px] text-white/20 italic p-4">No substitutes listed</p>';
    return;
  }

  players.forEach((player) => {
    const photoPath = player.playerPhoto
      ? BASE_PLAYER_PATH + player.playerPhoto
      : "/upload/players/placeholder.png";
    container.innerHTML += `
            <div class="flex items-center justify-between w-full hover:bg-white/5 rounded-lg transition-colors group">
                <div class="flex items-center gap-3">
                    <div class="relative w-14 h-14 rounded-lg overflow-hidden"
                         style="background: ${player.clubTheme}">
                        <img src="${photoPath}" 
                             class="w-full h-full object-contain pt-1"
                             onerror="this.src='/upload/players/placeholder.png'">
                    </div>

                    <div class="flex flex-col">
                        <h5 class="text-[11px] text-white font-bold text-start">
                            ${player.playerShortName}
                        </h5>
                        <p class="text-[11px] text-start text-white/40 font-medium">
                            <span class="mr-1">${player.playerNumber}</span>${player.position}
                        </p>
                    </div>
                </div>

                <div class="flex items-center gap-2 pr-2">
                    ${player.hasGoal ? '<img src="/upload/icons/ball.png" class="w-3 h-3 opacity-80">' : ""}
                    
                    ${
                      player.subInMinute
                        ? `
                        <div class="flex items-center gap-1 text-green-400 font-bold text-[10px] italic">
                            <svg class="w-3 h-3 fill-current" viewBox="0 0 24 24"><path d="M16 13h-3V3h-2v10H8l4 4 4-4zM4 19v2h16v-2H4z"/></svg>
                            ${player.subInMinute}'
                        </div>
                    `
                        : ""
                    }

                    ${player.hasYellowCard ? '<div class="w-2 h-3 bg-yellow-400 rounded-sm border border-black/20 shadow-sm"></div>' : ""}
                </div>
            </div>
        `;
  });
}
