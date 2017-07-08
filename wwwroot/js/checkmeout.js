
simpleCart({
    // array representing the format and columns of the cart,
    // see the cart columns documentation
    cartColumns: [
        //{ view: "image", attr: "thumb", label: false},
        { view: "remove", text: "X", label: "Remove" },
        { attr: "name", label: "Items" },
        { view: "currency", attr: "price", label: "Price"},
        //{ view: "decrement", label: false},
        { attr: "quantity", label: "Qty"},
        //{ view: "increment", label: false},
        { view: "currency", attr: "total", label: "SubTotal" },
        {view:"number", attr: "invid" , label: false } 
    ],
    // "div" or "table" - builds the cart as a
    // table or collection of divs
    cartStyle: "table",
    // how simpleCart should checkout, see the
    // checkout reference for more info
    checkout: {
        type: "PayPal" 
        ,email: "a2bman@hotmail.com"
        ,sandbox: false
    },
    // set the currency, see the currency
    // reference for more info
    currency: "CAD",
    // collection of arbitrary data you may want to store
    // with the cart, such as customer info
    data: {},
    // set the cart langauge
    // (may be used for checkout)
    language: "english-us",
    // array of item fields that will not be
    // sent to checkout
    excludeFromCheckout: [],
    // custom function to add shipping cost
    shippingCustom: null,
    // flat rate shipping option
    shippingFlatRate: 5,
    // added shipping based on this value
    // multiplied by the cart quantity
    shippingQuantityRate: 0,
    // added shipping based on this value
    // multiplied by the cart subtotal
    shippingTotalRate: 0,
    // tax rate applied to cart subtotal
    taxRate: 0.13,
    // true if tax should be applied to shipping
    taxShipping: true,
    // event callbacks
    beforeAdd            : null,
    afterAdd            : null,
    load                : null,
    beforeSave        : null,
    afterSave            : null,
    update            : null,
    ready            : null,
    checkoutSuccess    : orderPayed,
    checkoutFail        : orderFailed,
    beforeCheckout        : beforeCheckout,
    beforeRemove           : null
    });




    function  orderPayed(e){
    
        alert("Order Payed");
    }
    function  orderFailed(e){
    
        alert("Order failed");
    }
    function  beforeCheckout(e){
        var WaitingOnReply = true;
        loadDoc();
        //DebitWayInit();
        //e.preventDefault();
        // while(!WaitingOnReply==true){
        //    releaseEvents();
        // }
        
        // var person = prompt("We are delivering this to @User.Identity.Name", " @User.Identity.Name");

        // if (person == null || person == "") {
        //     txt = "User cancelled the prompt.";
        // } else {
        //     txt = "Hello " + person + "! How are you today?";
        // }
        
    }
   

    // Render the PayPal button

    // paypal.Button.render({

    //     // Set your environment

    //     env: 'sandbox', // sandbox | production

    //     // PayPal Client IDs - replace with your own
    //     // Create a PayPal app: https://developer.paypal.com/developer/applications/create

    //     client: {
    //         sandbox:    'AYlC1c8zVKliaivCxxM3wQN2ggOiXD3gx1Q7xDtA_6WhJAQxXAYPcsIg0glX4kwh4WgMxgrza770nZok',
    //         production: '<insert production client id>'
    //     },

    //     // Set to 'Pay Now'

    //     commit: true,

    //     // Wait for the PayPal button to be clicked

    //     payment: function(actions) {

    //         // Make a client-side call to the REST api to create the payment

    //         return actions.payment.create({
    //             transactions: [
    //                 {
    //                     amount: { total: '0.01', currency: 'USD' }
    //                 }
    //             ]
    //         });
    //     },

    //     // Wait for the payment to be authorized by the customer

    //     onAuthorize: function(data, actions) {

    //         // Execute the payment

    //         return actions.payment.execute().then(function() {
    //             window.alert('Payment Complete!');
    //         });
    //     }

    // }, '#paypal-button-container');
