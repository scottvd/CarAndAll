import {
  IconAdjustments,
  IconCalendarStats,
  IconCar,
  IconFileAnalytics,
  IconGauge,
  IconLock,
  IconNotes,
  IconPresentationAnalytics,
} from '@tabler/icons-react';
import { Code, Group, ScrollArea } from '@mantine/core';
import { LinksGroup } from './NavbarLinksGroup/NavbarLinksGroup';
// import { UserButton } from '../UserButton/UserButton';
// import { Logo } from './Logo';
import classes from './NavbarNested.module.css';
/*

- [ ] Rename all of the sections to their required parts
- [ ] Add stubs for most links
- [ ] Certain options should only be visible for certain roles (RBAC)
- [ ] Add user info at the bottom left corner of the navbar
- [ ] Look at responsive, make sure it's responsive
- [ ] implement user story
- [ ] make sure that is responsive
- [ ] write user story related stuff into document
- [ ] notify teammemberse
- [ ] ask for update on database/registration login (probably tomorrow dec 2 2024)

*/
const mockdata = [
  { label: 'Dashboard', icon: IconGauge },
  {
    label: 'Abonnementen',
    icon: IconNotes,
    initiallyOpened: true,
    links: [
      { label: 'Aanvragen abonnement', link: '/dashboard/abonnement' },
      { label: 'Status abonnementaanvraag', link: '/' },
      { label: 'Abonnement opzeggen', link: '/' },
      // { label: 'Real time', link: '/' },
    ],
  },
  {
    label: 'Voertuigen',
    icon: IconCar,
    links: [
      { label: 'Voertuigen bekijken', link: '/dashboard/voertuigen' }
    ],
  },
  { label: 'Analytics', icon: IconPresentationAnalytics },
  { label: 'Contracts', icon: IconFileAnalytics },
  { label: 'Settings', icon: IconAdjustments },
  {
    label: 'Security',
    icon: IconLock,
    links: [
      { label: 'Enable 2FA', link: '/' },
      { label: 'Change password', link: '/' },
      { label: 'Recovery codes', link: '/' },
    ],
  },
];

export function NavbarNested() {
  const links = mockdata.map((item) => <LinksGroup {...item} key={item.label} />);

  return (
    <nav className={classes.navbar}>


      <ScrollArea className={classes.links}>
        <div className={classes.linksInner}>{links}</div>
      </ScrollArea>

      <div className={classes.footer}>
        {/* <UserButton /> */}
      </div>
    </nav>
  );
}