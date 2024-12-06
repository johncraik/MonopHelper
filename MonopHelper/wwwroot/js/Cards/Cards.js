let BaseUrl = "../../Card/";
function ShowCards(decks){
    let selectedIndex = decks.options.selectedIndex;
    let deck = decks.options[selectedIndex].value;
    
    document.getElementById("DeckId").value = deck;
    FetchCards(deck);
}

function FetchCards(deckId){
    let url = BaseUrl + "CardsTablePartial?id=" + deckId;
    let model = document.getElementById("CardsTable");
    FetchPartial(url, model);
    
    url = BaseUrl + "AddCardsPartial?deckId=" + deckId;
    model = document.getElementById("AddCardsBtns");
    FetchPartial(url, model);
}

function MoveCards(copy){
    let id = document.getElementById("DeckId").value;
    let url = '/Cards/Move/' + id;
    
    if(copy === true){
        url += '?copy=' + copy;
    }
    
    location.assign(url);
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
                url: BaseUrl + 'RemoveCard?id=' + id,
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

function EditCardType(id, edit){
    let params = "?";
    let action = "Add";
    let successMsg = "Created"
    
    if(edit === true){
        action = "Edit";
        successMsg = "Renamed";
        params = '?id=' + id;
    }
    
    let url = BaseUrl + action;
    
    Swal.fire({
        title: action + " Card Type",
        icon: "info",
        input: "text",
        showCancelButton: true,
        confirmButtonText: "Save"
    }).then((result) => {
        if(result.isConfirmed){
            if(edit === true){
                params += '&';
            }
            params += 'name=' + result.value
            
            $.ajax({
                type: 'POST',
                url: url + 'CardType' + params,
                success: function (res){
                    Swal.fire("Card Type " + successMsg, "", "success").then((r) => {
                        let url = BaseUrl + "CardTypesPartial";
                        let model = document.getElementById("CardTypeTable");
                        FetchPartial(url, model);

                        let deckId = document.getElementById("DeckId").value;
                        url = BaseUrl + "AddCardsPartial?deckId=" + deckId;
                        model = document.getElementById("AddCardsBtns");
                        FetchPartial(url, model);
                    });
                }
            })

        }
    })
}

function DeleteCardType(id, name){
    Swal.fire({
        title: "Delete " + name + " Cards?",
        html: '<p>Cards with this card type will now have an undefined card type.<br/> These will need a card type set to be playable.</p>',
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: BaseUrl + 'RemoveCardType?id=' + id,
                success: function (res){
                    Swal.fire("Card Type Deleted", "", "success").then((r) => {
                        let url = BaseUrl + "CardTypesPartial";
                        let model = document.getElementById("CardTypeTable");
                        FetchPartial(url, model);

                        let deckId = document.getElementById("DeckId").value;
                        url = BaseUrl + "AddCardsPartial?deckId=" + deckId;
                        model = document.getElementById("AddCardsBtns");
                        FetchPartial(url, model);
                    });
                }
            })

        }
    })
}

function DeleteCardDeck(id, name){
    Swal.fire({
        title: "Delete " + name + " Card Deck?",
        html: '<p>Cards within this deck will now have an undefined card deck.<br/> These will need to be in a card deck to be playable.</p>',
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: BaseUrl + 'RemoveCardDeck?id=' + id,
                success: function (res){
                    Swal.fire("Card Type Deleted", "", "success").then((r) => {
                        let url = BaseUrl + "CardDecksPartial";
                        let model = document.getElementById("CardDeckTable");
                        FetchPartial(url, model);

                        document.getElementById("DeckId").value = "0";
                    });
                }
            })

        }
    })
}

function SetSelectedDeck(decks){
    let selectedIndex = decks.options.selectedIndex;
    document.getElementById("SelectedDeck").value = decks.options[selectedIndex].value;
}

function DeleteCardGame(id){
    Swal.fire({
        title: "Delete Card Game?",
        icon: "question",
        showCancelButton: true,
        confirmButtonText: "Yes"
    }).then((result) => {
        if(result.isConfirmed){
            $.ajax({
                type: 'POST',
                url: BaseUrl + 'RemoveCardGame?id=' + id,
                success: function (res){
                    Swal.fire("Card Game Deleted", "", "success").then((r) => {
                        let url = BaseUrl + "CardGamesPartial";
                        let model = document.getElementById("CardGameTable");
                        FetchPartial(url, model);
                    });
                }
            })

        }
    })
}