baseURL = "../../Turn";

function getPlayerPartial(id){
    let gameId = document.getElementById("game-id").innerText;
    let url = baseURL + "/GetPlayerPartial?id=" + id + "&gameId=" + gameId;
    let display = document.getElementById("player-partial");
    FetchPartial(url, display);
    
    url = baseURL + "/GetAlertPartial?id=" + id + "&gameId=" + gameId;
    display = document.getElementById("game-alerts");
    FetchPartial(url, display);
}

function changeNumbers(id, name)
{
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
                    $.ajax({
                        type: 'POST',
                        url: baseURL + '/ChangeNumber?id=' + id + '&d1=' + d1.value + '&d2=' + d2.value,
                        success: function (res){
                            let obj = JSON.parse(res);
                            if(obj.IsValid){
                                Swal.fire(name + "'s number has been set!", "", "success");
                                getPlayerPartial(obj.ReturnObj.GamePlayerId);
                            }
                            else {
                                Swal.fire({
                                    title: "Error",
                                    icon: "error",
                                    text: obj.ErrorMsg
                                });
                            }
                        }
                    })
                }
            })
        }
    })
}

function leaveJail(id, name, cost){
    Swal.fire({
        title: "Are you sure you want " + name + " to leave jail?",
        html: 'This will cost: <b><span class="money">₩</span>' + cost.toLocaleString() + '</b>',
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "No"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: baseURL + '/LeaveJail?id=' + id,
                success: function (res){
                    let obj = JSON.parse(res);
                    if(obj.IsValid){
                        Swal.fire(name + " has left jail!", "", "success");
                        getPlayerPartial(obj.ReturnObj.Id);
                    }
                    else {
                        Swal.fire({
                            title: "Error",
                            icon: "error",
                            text: obj.ErrorMsg
                        });
                    }
                }
            })
        }
    })
}

function resetJail(id, name){
    Swal.fire({
        title: "Are you sure you want to reset " + name + "'s jail cost?",
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "No"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: baseURL + '/ResetJail?id=' + id,
                success: function (res){
                    let obj = JSON.parse(res);
                    if(obj.IsValid){
                        Swal.fire(name + "'s jail cost has been reset!", "", "success");
                        getPlayerPartial(obj.ReturnObj.Id);
                    }
                    else {
                        Swal.fire({
                            title: "Error",
                            icon: "error",
                            text: obj.ErrorMsg
                        });
                    }
                }
            })
        }
    })
}

function claimTriple(id, name, amount){
    Swal.fire({
        title: "Are you sure you want " + name + " to claim a triple?",
        html: name + ' will receive: <b><span class="money">₩</span>' + amount.toLocaleString() + '</b>',
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "No"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: baseURL + '/ClaimTriple?id=' + id,
                success: function (res){
                    let obj = JSON.parse(res);
                    if(obj.IsValid){
                        Swal.fire(name + " has claimed a triple!", "", "success");
                        getPlayerPartial(obj.ReturnObj.Id);
                    }
                    else {
                        Swal.fire({
                            title: "Error",
                            icon: "error",
                            text: obj.ErrorMsg
                        });
                    }
                }
            })
        }
    })
}

function unmortgage(id, name, cost, playerId){
    let halfCost = cost / 2;
    let increase = halfCost * 0.1;
    let mortgageCost = halfCost + increase;
    mortgageCost = Math.round((mortgageCost / 10)) * 10;

    Swal.fire({
        title: "Are you sure you want to un-mortgage " + name + "?",
        html: 'This will cost: <b><span class="money">₩</span>' + mortgageCost.toLocaleString() + '</b> to un-mortgage.',
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "No"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: baseURL + '/Unmortgage?id=' + id,
                success: function (res){
                    let obj = JSON.parse(res);
                    if(obj.IsValid){
                        Swal.fire(name + " has been un-mortgaged!", "", "success").then((res) => {
                            getPlayerPartial(playerId);
                        });
                    }
                    else {
                        Swal.fire({
                            title: "Error",
                            icon: "error",
                            text: obj.ErrorMsg
                        });
                    }
                }
            })
        }
    })
}

function payLoan(type){
    let input = document.getElementById("repay-percent");
    if(type > 0){
        input.value = type;
    }
    else{
        input.value = 5;
    }
    
    let form = document.getElementById("pay-loans");
    form.submit();
}

function takeCard(gameId, typeId){
    let url = baseURL + "/GetCardPartial?gameId=" + gameId + "&typeId=" + typeId;
    let display = document.getElementById("card-partial");
    FetchPartial(url, display);
}

function unlockRes(id, name, playerId){
    Swal.fire({
        title: "Are you sure you want to unlock your reservation of " + name + "?",
        icon: "question",
        showDenyButton: true,
        confirmButtonText: "Yes",
        denyButtonText: "No"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: baseURL + '/UnlockReservation?propId=' + id + "&playerId=" + playerId,
                success: function (res){
                    let obj = JSON.parse(res);
                    if(obj.IsValid){
                        Swal.fire("Your reservation of " + name + " has been unlocked!", "", "success").then((res) => {
                            getPlayerPartial(playerId);
                        });
                    }
                    else {
                        Swal.fire({
                            title: "Error",
                            icon: "error",
                            text: obj.ErrorMsg
                        });
                    }
                }
            })
        }
    })
}