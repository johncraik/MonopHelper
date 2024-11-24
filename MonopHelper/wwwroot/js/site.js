function FetchPartial(url, model){
    fetch(url).then(data => {return data.text()}).then(body => {
        model.innerHTML = body;
        var scripts = model.querySelectorAll('script');
        for (let i = 0; i < scripts.length; i++) {
            if (scripts[i].type !== "text/x-template") {
                eval(scripts[i].innerHTML);
            }
        }
    });
}

function AddPlayer(){
    let player = EnsurePlayers();
    
    let playerDisplay = document.getElementById("GamePlayers");
    let url = "Game/NewGamePlayersPartial?players=" + encodeURIComponent(player);

    FetchPartial(url, playerDisplay);
}

function EnsurePlayers(){
    let inp = document.getElementById("PlayerName");
    let player = inp.value + "/#/";
    inp.value = "";
    let currPlayers = document.getElementsByClassName("player");

    for (let i = 0; i < currPlayers.length; i++){
        player += (currPlayers[i].innerText + "/#/")
    }

    document.getElementById("SetPlayers").value = player;
    return player;
}

function LoadGame(id){
    location.assign("/InGame/" + id)
}


function LeaveJail(id, name, cost){
    Swal.fire({
        title: "Are you sure you want " + name + " to leave jail?",
        html: 'This will cost: <b><span class="money">₩</span>' + cost.toLocaleString() + '</b>',
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "No"
    }).then((result) => {
        if(result.isConfirmed){
            Swal.fire(name + " has left jail!", "", "success").then((r) => {
                $.ajax({
                    type: 'POST',
                    url: '../Game/LeaveJail?id=' + id,
                    success: function (player){
                        let url = "../Game/InGamePlayerPartial?playerId=" + player;
                        let display = document.getElementById(player + "_Partial");
                        FetchPartial(url, display);
                    }
                })
            });
        }
    })
}

function SetNumber(id, name){
    Swal.fire({
        title: "Set " + name + "'s first dice:",
        icon: "info",
        input: "select",
        inputOptions: {
            1: "1",
            2: "2",
            3: "3",
            4: "4",
            5: "5",
            6: "6"
        },
        showDenyButton: true,
        confirmButtonText: "Set Number",
        denyButtonText: "Cancel"
    }).then((d1) => {
        if(d1.isConfirmed){
            Swal.fire({
                title: "Set " + name + "'s second dice:",
                icon: "info",
                input: "select",
                inputOptions: {
                    1: "1",
                    2: "2",
                    3: "3",
                    4: "4",
                    5: "5",
                    6: "6"
                },
                showDenyButton: true,
                confirmButtonText: "Set Number",
                denyButtonText: "Cancel"
            }).then((d2) => {
                if(d2.isConfirmed){
                    Swal.fire(name + "'s number has been set!", "", "success").then((r) => {
                        $.ajax({
                            type: 'POST',
                            url: '../Game/SetNumber?id=' + id + '&d1=' + d1.value + '&d2=' + d2.value,
                            success: function (player){
                                let url = "../Game/InGamePlayerPartial?playerId=" + player;
                                let display = document.getElementById(player + "_Partial");
                                FetchPartial(url, display);
                            }
                        })
                    });
                }
            })
            
        }
    })
}

function ClaimTriple(id, name, amount){
    Swal.fire({
        title: "Are you sure you want " + name + " to claim a triple?",
        html: name + ' will receive: <b><span class="money">₩</span>' + amount.toLocaleString() + '</b>',
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "No"
    }).then((result) => {
        if(result.isConfirmed){
            Swal.fire(name + " has claimed a triple!", "", "success").then((r) => {
                $.ajax({
                    type: 'POST',
                    url: '../Game/ClaimTriple?id=' + id,
                    success: function (player){
                        let url = "../Game/InGamePlayerPartial?playerId=" + player;
                        let display = document.getElementById(player + "_Partial");
                        FetchPartial(url, display);
                    }
                })
            });
        }
    })
}

function DeleteGame(form){
    Swal.fire({
        title: "Delete Game?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            Swal.fire("Game Deleted", "", "success").then((r) => {
                form.submit();
            });
        }
    })
}


function FetchGameTbl(txt, date){
    let tbl = document.getElementById("LoadGames");
    let url = "Game/LoadGamesPartial?txt=" + encodeURIComponent(txt) + "&dateTxt=" + date;

    fetch(url).then(data => {return data.text()}).then(body => {
        tbl.innerHTML = body;
        var scripts = tbl.querySelectorAll('script');
        for (let i = 0; i < scripts.length; i++) {
            if (scripts[i].type !== "text/x-template") {
                eval(scripts[i].innerHTML);
            }
        }
    });
}

function Search(search){
    let txt = search.value;
    let date = document.getElementById("DateFilter").value;
    FetchGameTbl(txt, date);
}

function FilterDate(picker){
    let date = picker.value;
    let txt = document.getElementById("SearchTxt").value;
    FetchGameTbl(txt, date);
}