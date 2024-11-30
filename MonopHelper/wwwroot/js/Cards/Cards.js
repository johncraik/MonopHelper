function ShowCards(decks){
    let selectedIndex = decks.options.selectedIndex;
    let deck = decks.options[selectedIndex].value;
    
    document.getElementById("DeckId").value = deck;
    FetchCards(deck);
}

function FetchCards(deckId){
    let url = "../../Card/CardsTablePartial?id=" + deckId;
    let model = document.getElementById("CardsTable");
    FetchPartial(url, model);
}

function SetUploadType(id, name){
    document.getElementById("TypeId").value = id;
    document.getElementById("CardTypeName").innerText = name;
}

function DeleteCard(id){
    Swal.fire({
        title: "Delete Card?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: '../../Card/RemoveCard?id=' + id,
                success: function (res){
                    Swal.fire("Card Deleted", "", "success").then((r) => {
                        let deck = document.getElementById("DeckId").value;
                        FetchCards(deck);
                    });
                }
            })
            
        }
    })
}

function EditCardType(id, name){
    Swal.fire({
        title: "Rename Card Type",
        icon: "info",
        input: "text",
        name,
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: '../../Card/EditCardType?id=' + id + '&name=' + result.value,
                success: function (res){
                    Swal.fire("Card Type Renamed", "", "success").then((r) => {
                        let url = "../../Card/CardTypesPartial";
                        let model = document.getElementById("CardTypeTable");
                        FetchPartial(url, model);
                    });
                }
            })

        }
    })
}

function DeleteCardType(id){
    Swal.fire({
        title: "Delete Card Type?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: '../../Card/RemoveCardType?id=' + id,
                success: function (res){
                    Swal.fire("Card Type Deleted", "", "success").then((r) => {
                        let url = "../../Card/CardTypesPartial";
                        let model = document.getElementById("CardTypeTable");
                        FetchPartial(url, model);
                    });
                }
            })

        }
    })
}