function AddPlayer(){
    let player = EnsurePlayers();
    
    let playerDisplay = document.getElementById("GamePlayers");
    let url = "Game/NewGamePlayersPartial?players=" + encodeURIComponent(player);

    fetch(url).then(data => {return data.text()}).then(body => {
        playerDisplay.innerHTML = body;
        var scripts = playerDisplay.querySelectorAll('script');
        for (let i = 0; i < scripts.length; i++) {
            if (scripts[i].type !== "text/x-template") {
                eval(scripts[i].innerHTML);
            }
        }
    });
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


function LeaveJail(id, name){
    Swal.fire({
        title: "Are you sure you want " + name + " to leave jail?",
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
                    success: function (game){
                        location.assign("/InGame/" + game);
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
                            success: function (game){
                                location.assign("/InGame/" + game);
                            }
                        })
                    });
                }
            })
            
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
