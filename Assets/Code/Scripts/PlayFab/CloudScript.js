var defaultCatalog = "CharacterClasses";
var STAR_CURRENCY_CODE = "SC";
var RED_STAR_CURRENCY_CODE = "HC";

///////////////////////// Cloud Script Handler Functions /////////////////////////
handlers.AddStar = function AddStar(args){
    var addStarRequest = {
    PlayFabId: currentPlayerId,
    VirtualCurrency: STAR_CURRENCY_CODE,
    Amount: args.amount
    };
    return server.AddUserVirtualCurrency(addStarRequest);
}

handlers.SubtractStar = function SubtractStar(args){
    var subtractStarRequest = {
    PlayFabId: currentPlayerId,
    VirtualCurrency: STAR_CURRENCY_CODE,
    Amount: args.amount
    };
    return server.SubtractUserVirtualCurrency(subtractStarRequest);
}

handlers.AddRedStar = function AddRedStar(args){
    var addRedStarRequest = {
    PlayFabId: currentPlayerId,
    VirtualCurrency: RED_STAR_CURRENCY_CODE,
    Amount: args.amount
    };
    return server.AddUserVirtualCurrency(addRedStarRequest);
}

handlers.SubtractRedStar = function SubtractRedStar(args){
    var subtractRedStarRequest = {
    PlayFabId: currentPlayerId,
    VirtualCurrency: RED_STAR_CURRENCY_CODE,
    Amount: args.amount
    };
    return server.SubtractUserVirtualCurrency(subtractRedStarRequest);
}