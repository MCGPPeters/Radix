async function getMedia(constraints) {
    try {
        let stream = null;
        stream = await navigator.mediaDevices.getUserMedia(constraints);
        const localVideo = document.getElementById("local-video");
        if (localVideo) {
            localVideo.srcObject = stream;
            localVideo.mediaRecorder = new MediaRecorder(videoElement.mediaStream);
            localVideo.volume = 0;
            localVideo.mediaRecorder.ondataavailable = async (e) => {
                let uintArr = new Uint8Array(await new Response(e.data).arrayBuffer());
                let buffer = Array.from(uintArr);
                await DotNet.invokeMethodAsync("ReceiveData", buffer);
            };

            localVideo.play();
        }
    } catch (error) {
        console.warn(error.message);
    }
}
