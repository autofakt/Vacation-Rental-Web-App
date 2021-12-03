redirectToCheckout = function (sessionId) {
    var stripe = Stripe('pk_test_51K2MBJCYtJ08h9bDIu6DghVg6J2qM0eEmSxuNxUhnhfC4cS8LZu0Fq009Cdxjz44gb8ADn0c7t39eYC79DLnZNdi00avMMtIsI');
    stripe.redirectToCheckout({
        sessionId: sessionId
    });
};