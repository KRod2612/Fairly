import * as Yup from 'yup';

let orgSchema = Yup.object().shape({
    organizationTypeId: Yup.number().required("Type Required"),
    name: Yup.string().min(2).required("Name Required"),
    headline: Yup.string().min(2).required("Headline Required"),
    description: Yup.string().min(2).required("Description Required"),
    logo: Yup.string().min(2).required("Logo Required"),
    locationId: Yup.number().required("Location Required"),
    phone: Yup.string().matches(/^((\\+[1-9]{1,4}[ \\-]*)|(\\([0-9]{2,3}\\)[ \\-]*)|([0-9]{2,4})[ \\-]*)*?[0-9]{3,4}?[ \\-]*[0-9]{3,4}?$/, 'Phone number not valid').required("Phone number required"),
    siteUrl: Yup.string().min(2).required("Url Required"),
});
export default {orgSchema}