import XlsxLoader from "./components/XlsxLoader";
import { Schedule } from "./components/Schedule";

const AppRoutes = [
  {
    index: true,
    element: <Schedule />
  },
  {
    path: '/loader',
    element: <XlsxLoader />
  }
];

export default AppRoutes;
