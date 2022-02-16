var ValidateForm = function () {

    function isValid(formId,fields) {

        var validation = FormValidation.formValidation(
            KTUtil.getById(formId),
            {
                fields: fields,
                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    //defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
                    bootstrap: new FormValidation.plugins.Bootstrap()
                }
            }
        );

        return validation;
         
    }
    return {
        // public functions
        IsValid: function (formId, fields) {
            return isValid(formId, fields);
        }
    };
}();

