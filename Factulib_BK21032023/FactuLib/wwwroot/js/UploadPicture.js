
class UploadPicture {
    archivo(evt, id) {
        let files = evt.target.files; // FilesList Object
        let f = files[0];
        if (f.type.match('image.*')) {
            let reader = new FileReader();
            reader.onload = ((theFile) => {
                return (e) => {
                    document.getElementById(id).innerHTML = ['<img class ="imageUser" src="', e.target.result, '" title="', escape(theFile.name), '"/>'].join('')
                }
            })(f);
            reader.readAsDataURL(f);
        }
    }
}
