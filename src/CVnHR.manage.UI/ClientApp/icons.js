import { library } from '@fortawesome/fontawesome-svg-core'
// Official documentation available at: https://github.com/FortAwesome/vue-fontawesome
import {
  faEnvelope, faGraduationCap, faHome, faList, faSpinner, faCog, faBook
} from '@fortawesome/free-solid-svg-icons'
import {
  faFontAwesome, faMicrosoft, faVuejs
} from '@fortawesome/free-brands-svg-icons'
import {
  faPlusSquare, faMinusSquare
} from '@fortawesome/free-regular-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

// If not present, it won't be visible within the application. Considering that you
// don't want all the icons for no reason. This is a good way to avoid importing too many
// unnecessary things.
library.add(
  faEnvelope, faGraduationCap, faHome, faList, faSpinner, faCog, faBook,

  // Brands
  faFontAwesome, faMicrosoft, faVuejs,

  // Regular
  faPlusSquare, faMinusSquare
)

export {
  FontAwesomeIcon
}
