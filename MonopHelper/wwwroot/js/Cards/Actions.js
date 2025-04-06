function ShowPlayCondition(toggle){
    let condd = document.getElementById('con-dd');
    condd.hidden = toggle.checked !== true;
}

function DeleteActions(){
    Swal.fire({
        title: 'Delete Actions?',
        text: 'This cannot be undone.',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((res) => {
        if(res.isConfirmed) {
            let frm = document.getElementById('delete-form');
            frm.submit();
        }
    })
}

function SetGroup(groupId){
    let group = "New Group"
    if(groupId !== "0"){
        group = "Group " + groupId;
    }
    
    document.getElementById('add-group').innerText = group;
    document.getElementById('group-id').value = groupId;
}

function SetInputLabel(id, opt1, opt2){
    let label =  document.getElementById(id);
    let name= label.innerText;
    if(name === opt1){
        name = opt2;
    }
    else{
        name = opt1;
    }
    label.innerText = name;
}

function SetPayLabels(){
    SetInputLabel('pay-val', 'Pay', 'Receive');
    SetInputLabel('payto', 'Pay To', 'Receive From');
}

function SetPropLabels(){
    SetInputLabel('source', 'Take Property From', 'Return Property To')
}

function SetResetInput(list){
    let valInp = document.getElementById('reset-val');
    valInp.hidden = list.value !== "1";
}

function SetEventLabel(list){
    let val = list.value;
    let label = document.getElementById('event-val-label');
    let valDiv = document.getElementById('event-val');
    let fp = document.getElementById('fp-event');
    let go = document.getElementById('go-event');
    
    if(val === "0" || val === "1" || val === "3"){
        valDiv.hidden = false;
        label.innerText = "Number of Turns"
        fp.hidden = true;
        go.hidden = true;
    }
    else if(val === "2"){
        valDiv.hidden = false;
        label.innerText = "Number of Properties"
        fp.hidden = true;
        go.hidden = true;
    }
    else if(val === "4" || val === "5" || val === "6"){
        valDiv.hidden = false;
        label.innerText = "Rent Multiplier"
        fp.hidden = true;
        go.hidden = true;
    }
    else if(val === "7"){
        valDiv.hidden = true;
        fp.hidden = false;
        go.hidden = true;
    }
    else{
        valDiv.hidden = false;
        label.innerText = "Pay/Receive on GO"
        fp.hidden = true;
        go.hidden = false;
    }
}