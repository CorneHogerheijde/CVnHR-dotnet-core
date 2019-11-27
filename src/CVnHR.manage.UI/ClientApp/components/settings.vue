<template>
  <div>
    <h1>Settings</h1>

    <icon v-if="!currentSettings" icon="spinner" pulse />

    <div v-if="currentSettings">
      <!--UPLOAD-->
      <form enctype="multipart/form-data" novalidate v-if="true">
        <div class="dropbox">
          <input type="file"
                 class="input-file"
                 accept=".pfx"
                 @change="setPassword($event.target.files)" />
          <!-- uploadCertificate($event.target.files); fileCount = $event.target.files.length -->
          <p v-if="true">
            Upload certificate by dragging a pfx file here<br> or click to browse
          </p>
          <p v-if="false">
            Uploading certificate...
          </p>
        </div>
      </form>

      <h3>Current Certificate</h3>
      <p v-if="currentSettings.certificate">{{currentSettings.certificate}}</p>
      <p v-else>Upload a certificate to start working with this application.</p>

      <h3>Kvk Api Settings</h3>
      <label>
        KvK API key
        <input v-model="currentSettings.kvkApiSettings.apiKey" />
      </label>
      <label>
        Kvk base url
        <input v-model="currentSettings.kvkApiSettings.baseUrl" />
      </label>
      <label>
        Kvk search url
        <input v-model="currentSettings.kvkApiSettings.searchUrl" />
      </label>
      <label>
        Kvk profile url
        <input v-model="currentSettings.kvkApiSettings.profileUrl" />
      </label>

      <h3>HR Dataservice Settings</h3>
      <label>
        <i>Klantreferentie</i>
        <input v-model="currentSettings.hrDataServiceSettings.klantReferentie" />
      </label>
      <button @click="updateSettings(currentSettings)">update settings</button>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="password-modal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLongTitle">Set password for certificate</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <input type="password" v-model="password" @keyup.enter="uploadCertificateAndPassword" />
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-primary" data-dismiss="modal" aria-label="Close"
                    @click="uploadCertificateAndPassword">
              Save password
            </button>
          </div>
        </div>
      </div>
    </div>

  </div> 
</template>

<script>
  import { mapActions, mapState } from 'vuex'
  import $ from 'jquery'

  export default {
    data() {
      return {
        password: null,
        files: null
      }
    },

    computed: {
      ...mapState({
        currentSettings: state => state.settings
      })
    },

    methods: {
      ...mapActions(['updateSettings', 'uploadCertificate']),
      setPassword(files) {
        if (files.length > 1) {
          window.alert("Only upload one pfx file!")
          return
        }

        this.files = files;
        if (files.length > 0) {
          $('#password-modal').modal('show')
        }
      },
      uploadCertificateAndPassword() {
        this.files.password = this.password
        this.uploadCertificate(this.files)
        this.closeModal()
      },
      closeModal() {
        $('#password-modal').modal('hide')
      }
    },
    mounted() {
      $('#password-modal')
        .on('hide.bs.modal', () => { $('form').trigger("reset") })
    }
  }
</script>

<style>
  label, input {
    width: 100%;
  }

  .dropbox {
    outline: 2px dashed grey; /* the dash box */
    outline-offset: -10px;
    background: lightgray;
    color: dimgray;
    padding: 10px 10px;
    min-height: 200px; /* minimum height */
    position: relative;
    cursor: pointer;
  }

  .input-file {
    opacity: 0; /* invisible but it's there! */
    width: 100%;
    height: 200px;
    position: absolute;
    cursor: pointer;
  }

  .dropbox:hover {
    background: gray; /* when mouse over to the drop zone, change color */
  }

  .dropbox p {
    font-size: 1.2em;
    text-align: center;
    padding: 50px 0;
  }
</style>
