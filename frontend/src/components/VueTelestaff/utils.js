import moment from "moment";

const RANK_MAPPINGS = [
  {
    rank: "medic",
    titles: ["medic", "attendant a"],
    icon: "user-md",
  },
  {
    rank: "officer",
    titles: [
      "captain",
      "lieutenant",
      "officer",
      "battalion aide",
      "battalion chief",
    ],
    icon: "star",
  },
  {
    rank: "driver",
    titles: ["dot", "doe"],
    icon: "car",
  },
  {
    // We use "" in titles to make it the default, this will *always* match
    rank: "firefighter",
    titles: ["firefighter", "attendant b", ""],
    icon: "fire",
  },
];

const utils = {
  station: null,

  fetchRoster(opts = {}) {
    utils.station = opts.station;
    return fetch(`${opts.url}${opts.date}`)
      .then((resp) => resp.json())
      .then(utils.mapRoster);
  },

  mapRoster(data) {
    console.log(data);
    const {
      data: {
        Date: [
          {
            title: dataDate,
            Institution: [
              {
                Agency: [
                  {
                    Batallion: [{ Shift: shifts }],
                  },
                ],
              },
            ],
          },
        ],
      },
    } = data;

    const shiftDate = moment(dataDate, "dddd, MMMM Do YYYY");

    const thisShift = utils.getShift(shiftDate);

    const shift = shifts.filter(
      (shift) => shift.title === `Ops ${thisShift.toUpperCase()} Shift`
    )[0];

    const stationRoster = shift["Station"].filter(
      (station) => station.title === utils.station
    )[0];

    return {
      day: utils.getTodayOrDayName(shiftDate),
      date: shiftDate,
      shift: shift.title.split(" ")[1].toLowerCase(),
      station: stationRoster.title,
      units: stationRoster["Unit"].filter(
        (unit) => unit.title !== "{off roster}"
      ),
      activities: stationRoster["Unit"].reduce((activites, unit) => {
        if (unit.notes !== "") {
          activites.push(
            {
              unit: unit.title,
              activity: unit.notes,
            },
            []
          );
        }
        return activites;
      }, []),
    };
  },

  getDeltaDay(delta = 1) {
    const tomorrow = moment().add(delta, "days");
    return tomorrow.format("YYYYMMDD");
  },

  getTodayOrDayName(date) {
    if (date.isSame(moment(), "day")) {
      return "Today";
    }
    return date.format("dddd");
  },

  getRank(str) {
    str = str.toLowerCase() || "";

    // Prepare str by removing '.', '+', and leading + trailing white
    //      space, and making everything lower case
    str = str.replace(/[.+]/g, "").trim();

    const retVal = RANK_MAPPINGS.find((rank) => {
      for (let tdx in rank.titles) {
        if (str.includes(rank.titles[tdx])) {
          return true;
        }
      }
      return false;
    });
    return retVal;
  },

  parseShiftTimes(str) {
    // 05/30/2017 07:00 AM
    str = str || "";
    str = str.slice(10);
    var parts = str.trim().split(" ");
    if (parts[1] == "PM") {
      var p = parts[0].split(":");
      if (p[0] != 12) {
        parts[0] = (parseInt(p[0]) + 12).toString() + ":" + p[1];
      }
    }
    return parts[0];
  },

  getShift(date) {
    date = date || moment();
    const shiftMap = {
      // 2017/00/03 - 2017/00/05 - 2017/00/07
      a: [moment([2017, 0, 3]), moment([2017, 0, 5]), moment([2017, 0, 7])],
      // 2017/00/06 - 2017/00/08 - 2017/00/10
      b: [moment([2017, 0, 6]), moment([2017, 0, 8]), moment([2017, 0, 10])],
      // 2017/00/09 - 2017/00/11 - 2017/00/13
      c: [moment([2017, 0, 9]), moment([2017, 0, 11]), moment([2017, 0, 13])],
    };

    for (const shiftKey in shiftMap) {
      for (const shiftStart of shiftMap[shiftKey]) {
        const deltaDate = date.diff(shiftStart, "days");
        if (deltaDate % 9 === 0) {
          return shiftKey;
        }
      }
    }
  },
};

export default utils;
